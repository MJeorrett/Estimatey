using Estimatey.Core.Entities;
using Estimatey.Infrastructure.DevOps.Models;
using Estimatey.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Estimatey.Infrastructure.DevOps;

internal class DevOpsWorkItemSynchronizer : BackgroundService
{
    private const int PauseBetweenSyncsMilliseconds = 8000;

    private readonly IServiceProvider _services;
    private readonly ILogger _logger;
    private readonly DevOpsClient _devOpsClient;

    public DevOpsWorkItemSynchronizer(
        IServiceProvider services,
        ILogger<DevOpsWorkItemSynchronizer> logger,
        DevOpsClient devOpsClient)
    {
        _services = services;
        _logger = logger;
        _devOpsClient = devOpsClient;
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

        foreach (var project in projects)
        {
            await SyncProject(dbContext, project);

            if (stoppingToken.IsCancellationRequested) return;
        }
    }

    private async Task SyncProject(ApplicationDbContext dbContext, ProjectEntity project)
    {
        _logger.LogInformation("Syncing project {projectName}.", project.DevOpsProjectName);

        var workItemRevisionsDto = await _devOpsClient.GetWorkItemRevisions(project.DevOpsProjectName, project.DevOpsContinuationToken)
;

        await ProcessWorkItemRevisions(dbContext, project, workItemRevisionsDto);

        _logger.LogInformation("Successfully synced project {projectName}.", project.DevOpsProjectName);

        if (!workItemRevisionsDto.isLastBatch)
        {
            await SyncProject(dbContext, project);
        }
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
