using Estimatey.Core.Entities;
using Estimatey.Infrastructure.DevOps.Models;
using Estimatey.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Estimatey.Infrastructure.DevOps;

internal class WorkItemSynchronizationService
{
    private const int PauseBetweenSyncsMilliseconds = 8000;

    private readonly ILogger _logger;
    private readonly DevOpsClient _devOpsClient;
    private readonly ApplicationDbContext _dbContext;

    public WorkItemSynchronizationService(
        ILogger<WorkItemSynchronizationService> logger,
        DevOpsClient devOpsClient,
        ApplicationDbContext dbContext)
    {
        _logger = logger;
        _devOpsClient = devOpsClient;
        _dbContext = dbContext;
    }

    public async Task SyncProject(ProjectEntity project)
    {
        _logger.LogInformation("Syncing work items for project {projectName}.", project.DevOpsProjectName);

        var workItemRevisionsDto = await _devOpsClient.GetWorkItemRevisionsBatch(project.DevOpsProjectName, project.WorkItemsContinuationToken);

        await ProcessWorkItemRevisions(project, workItemRevisionsDto);

        _logger.LogInformation("Successfully synced work items for project {projectName}.", project.DevOpsProjectName);

        if (!workItemRevisionsDto.IsLastBatch)
        {
            await SyncProject(project);
        }
    }

    private async Task ProcessWorkItemRevisions(
        ProjectEntity project,
        WorkItemRevisionsDto workItemRevisionsDto)
    {
        var allTagNames = workItemRevisionsDto.Values
            .SelectMany(_ => _.Tags)
            .ToList();

        _logger.LogDebug("Upserting {tagNamesCount} tag names.", allTagNames.Count);

        await UpsertTags(allTagNames);

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

                        var existingFeature = await _dbContext.Features
                            .Include(_ => _.Tags)
                            .FirstOrDefaultAsync(_ => _.DevOpsId == workItemRevision.Id);

                        await UpsertWorkItem(existingFeature, workItemRevision);

                        break;
                    }

                case "User Story":
                    {
                        _logger.LogDebug("Upserting User Story {id}.", workItemRevision.Id);

                        var existingUserStory = await _dbContext.UserStories
                            .Include(_ => _.Tags)
                            .FirstOrDefaultAsync(_ => _.DevOpsId == workItemRevision.Id);

                        await UpsertWorkItem(existingUserStory, workItemRevision);

                        break;
                    }

                case "Task":
                    {
                        _logger.LogDebug("Upserting Ticket {id}.", workItemRevision.Id);

                        var existingUserStory = await _dbContext.Tickets
                            .Include(_ => _.Tags)
                            .FirstOrDefaultAsync(_ => _.DevOpsId == workItemRevision.Id);

                        await UpsertWorkItem(existingUserStory, workItemRevision);

                        break;
                    }
            }
        }

        _logger.LogInformation("Setting continuation token for project {projectName} to '{continuationToken}'.", project.DevOpsProjectName, workItemRevisionsDto.ContinuationToken);

        project.WorkItemsContinuationToken = workItemRevisionsDto.ContinuationToken;

        await _dbContext.SaveChangesAsync();
    }

    private async Task UpsertWorkItem<T>(
        T? existingWorkItem,
        WorkItemRevision workItemRevision)
        where T: WorkItemEntity, new()
    {
        var tagEntities = await _dbContext.Tags.
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

            _dbContext.Add(new T()
            {
                DevOpsId = workItemRevision.Id,
                Title = workItemRevision.Fields.Title,
                State = workItemRevision.Fields.State,
                Tags = tagEntities,
            });
        }
    }

    private async Task UpsertTags(IEnumerable<string> tagNames)
    {
        foreach (var tagName in tagNames.Distinct())
        {
            var existingTag = await _dbContext.Tags
                .FirstOrDefaultAsync(_ => _.Name == tagName);

            if (existingTag is null)
            {
                _dbContext.Tags.Add(new()
                {
                    Name = tagName,
                });
            }
        }

        await _dbContext.SaveChangesAsync();
    }
}
