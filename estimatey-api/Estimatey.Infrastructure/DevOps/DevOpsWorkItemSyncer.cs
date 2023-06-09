using Azure.Core;
using Azure.Identity;
using Estimatey.Core.Entities;
using Estimatey.Infrastructure.DevOps.Models;
using Estimatey.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.Services.Client;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace Estimatey.Infrastructure.DevOps;

internal class DevOpsWorkItemSynchronizer : BackgroundService
{
    private const int PauseBetweenSyncsMilliseconds = 8000;

    private readonly IServiceProvider _services;
    private readonly ILogger _logger;

    private readonly string _azureAadtenantId;
    private readonly string _clientId;
    private readonly string _clientSecret;
    private readonly string _organizationName;

    public DevOpsWorkItemSynchronizer(
        IServiceProvider services,
        ILogger<DevOpsWorkItemSynchronizer> logger,
        IOptions<DevOpsOptions> options)
    {
        _services = services;
        _logger = logger;

        _organizationName = options.Value.OrganizationName;
        _azureAadtenantId = options.Value.AzureAadTenantId;
        _clientId = options.Value.ClientId;
        _clientSecret = options.Value.ClientSecret;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Syncing projects.");

            await SyncAllProjects(stoppingToken);

            _logger.LogInformation("Finished syncing projects.");

            if (stoppingToken.IsCancellationRequested) return;

            Thread.Sleep(PauseBetweenSyncsMilliseconds);
        }
    }

    private async Task SyncAllProjects(CancellationToken stoppingToken)
    {
        using var scope = _services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var projects = await dbContext.Projects.ToListAsync();

        _logger.LogInformation("Syncing {projectsCount} projects.", projects.Count);

        var credential = new ClientSecretCredential(_azureAadtenantId, _clientId, _clientSecret);
        var tokenRequestContext = new TokenRequestContext(VssAadSettings.DefaultScopes);
        var accessToken = await credential.GetTokenAsync(tokenRequestContext, CancellationToken.None);

        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.Token);

        foreach (var project in projects)
        {
            await SyncProject(httpClient, dbContext, project);

            if (stoppingToken.IsCancellationRequested) return;
        }
    }

    private async Task SyncProject(HttpClient httpClient, ApplicationDbContext dbContext, ProjectEntity project)
    {
        _logger.LogInformation("Syncing project {projectName}.", project.DevOpsProjectName);

        string uri = BuildGetWorkItemRevisionsUri(project);

        var workItemRevisionsResponse = await httpClient.GetAsync(uri);

        _logger.LogDebug("Received work item revisions response from DevOps for project {projectName}:\n{response}", await workItemRevisionsResponse.Content.ReadAsStringAsync(), project.DevOpsProjectName);

        if (workItemRevisionsResponse.StatusCode != HttpStatusCode.OK)
        {
            _logger.LogWarning("Fetching work item revisions from DevOps returned unexpected status code {statusCode} and body {responseBody} when syncing project {projectName}.", workItemRevisionsResponse.StatusCode, await workItemRevisionsResponse.Content.ReadAsStringAsync(), project.DevOpsProjectName);

            return;
        }

        var workItemRevisionsDto = await workItemRevisionsResponse.Content.ReadFromJsonAsync<WorkItemRevisionsDto>() ??
            throw new Exception("Failed to parse response from get work item revisions response.");

        _logger.LogInformation("Received {numberOfWorkItemRevisions} work item revisions from DevOps for project {projectName}.", workItemRevisionsDto.Values.Count, project.DevOpsProjectName);

        await ProcessWorkItemRevisions(dbContext, project, workItemRevisionsDto);

        _logger.LogInformation("Successfully synced project {projectName}.", project.DevOpsProjectName);

        if (!workItemRevisionsDto.isLastBatch)
        {
            await SyncProject(httpClient, dbContext, project);
        }
    }

    private string BuildGetWorkItemRevisionsUri(ProjectEntity project)
    {
        var projectName = project.DevOpsProjectName;
        var continuationToken = project.DevOpsContinuationToken;

        var uri = $"https://dev.azure.com/{_organizationName}/{projectName}/_apis/wit/reporting/workitemrevisions?api-version=4.1";

        if (!string.IsNullOrWhiteSpace(continuationToken))
        {
            uri += $"&continuationToken={continuationToken}";
        }

        return uri;
    }

    private async Task ProcessWorkItemRevisions(
        ApplicationDbContext dbContext,
        ProjectEntity project,
        WorkItemRevisionsDto workItemRevisionsDto)
    {
        var allTagNames = workItemRevisionsDto.Values
            .SelectMany(_ => _.Tags)
            .ToList();

        _logger.LogDebug("Upserting {tagNamesCount} tag names.", allTagNames.Count);

        await UpsertTags(dbContext, allTagNames);

        var workItemRevisions = workItemRevisionsDto.Values
            .GroupBy(_ => _.Id)
            .Select(_ => _.OrderByDescending(_ => _.Rev).First())
            .ToList();

        foreach (var workItemRevision in workItemRevisions)
        {
            _logger.LogDebug("Processing work item revision:\n{workItemRevisionJson}.", JsonSerializer.Serialize(workItemRevision));

            switch (workItemRevision.Fields.WorkItemType)
            {
                case "Feature":
                    {
                        _logger.LogDebug("Upserting Feature {id}.", workItemRevision.Id);

                        var existingFeature = await dbContext.Features
                            .Include(_ => _.Tags)
                            .FirstOrDefaultAsync(_ => _.DevOpsId == workItemRevision.Id);

                        await UpsertWorkItem(dbContext, existingFeature, workItemRevision);

                        break;
                    }

                case "User Story":
                    {
                        _logger.LogDebug("Upserting User Story {id}.", workItemRevision.Id);

                        var existingUserStory = await dbContext.UserStories
                            .Include(_ => _.Tags)
                            .FirstOrDefaultAsync(_ => _.DevOpsId == workItemRevision.Id);

                        await UpsertWorkItem(dbContext, existingUserStory, workItemRevision);

                        break;
                    }

                case "Task":
                    {
                        _logger.LogDebug("Upserting Ticket {id}.", workItemRevision.Id);

                        var existingUserStory = await dbContext.Tickets
                            .Include(_ => _.Tags)
                            .FirstOrDefaultAsync(_ => _.DevOpsId == workItemRevision.Id);

                        await UpsertWorkItem(dbContext, existingUserStory, workItemRevision);

                        break;
                    }
            }
        }

        _logger.LogInformation("Setting continuation token for project {projectName} to '{continuationToken}'.", project.DevOpsProjectName, workItemRevisionsDto.ContinuationToken);

        project.DevOpsContinuationToken = workItemRevisionsDto.ContinuationToken;

        await dbContext.SaveChangesAsync();
    }

    private async Task UpsertWorkItem<T>(
        ApplicationDbContext dbContext,
        T? existingWorkItem,
        WorkItemRevision workItemRevision)
        where T: WorkItemEntity, new()
    {
        var tagEntities = await dbContext.Tags.
            Where(_ => workItemRevision.Tags.Contains(_.Name))
            .ToListAsync();

        if (existingWorkItem is not null)
        {
            _logger.LogDebug("Work Item with id {id} exists so updating it.", workItemRevision.Id);

            existingWorkItem.Title = workItemRevision.Fields.Title;
            existingWorkItem.State = workItemRevision.Fields.State;
            existingWorkItem.Tags = tagEntities;
        }
        else
        {
            _logger.LogDebug("Work Item with id {id} does not exist so creating it.", workItemRevision.Id);

            dbContext.Add(new T()
            {
                DevOpsId = workItemRevision.Id,
                Title = workItemRevision.Fields.Title,
                State = workItemRevision.Fields.State,
                Tags = tagEntities,
            });
        }
    }

    private static async Task UpsertTags(
        ApplicationDbContext dbContext,
        IEnumerable<string> tagNames)
    {
        foreach (var tagName in tagNames.Distinct())
        {
            var existingTag = await dbContext.Tags
                .FirstOrDefaultAsync(_ => _.Name == tagName);

            if (existingTag is null)
            {
                dbContext.Tags.Add(new()
                {
                    Name = tagName,
                });
            }
        }

        await dbContext.SaveChangesAsync();
    }
}
