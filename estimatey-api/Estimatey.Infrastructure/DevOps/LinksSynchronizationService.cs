﻿using Estimatey.Core.Entities;
using Estimatey.Infrastructure.DevOps.Models;
using Estimatey.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Threading;

namespace Estimatey.Infrastructure.DevOps;

internal class LinksSynchronizationService
{
    public readonly ILogger _logger;

    public readonly DevOpsClient _devOpsClient;

    public readonly ApplicationDbContext _dbContext;

    private static readonly ConcurrentDictionary<int, SemaphoreSlim> _semaphores = new();

    public LinksSynchronizationService(
        ILogger<LinksSynchronizationService> logger,
        DevOpsClient devOpsClient,
        ApplicationDbContext dbContext)
    {
        _logger = logger;
        _devOpsClient = devOpsClient;
        _dbContext = dbContext;
    }

    public async Task SyncProject(ProjectEntity project)
    {
        var workItemLinksBatch = await SyncProjectInternal(project);

        if (!workItemLinksBatch.IsLastBatch)
        {
            await SyncProject(project);
        }
    }

    private async Task<WorkItemLinksBatch> SyncProjectInternal(ProjectEntity project)
    {
        var semaphore = _semaphores.GetOrAdd(project.Id, _ => new SemaphoreSlim(1, 1));

        await semaphore.WaitAsync();

        try
        {
            _logger.LogInformation("Syncing links for project {projectName}.", project.DevOpsProjectName);

            var workItemLinksBatch = await _devOpsClient.GetWorkItemLinksBatch(project.DevOpsProjectName, project.LinksContinuationToken)
    ;
            await ProcessWorkItemLinks(project, workItemLinksBatch);

            _logger.LogInformation("Successfully synced links for project {projectName}.", project.DevOpsProjectName);
            return workItemLinksBatch;

        }
        finally
        {
            semaphore.Release();
        }
    }

    private async Task ProcessWorkItemLinks(ProjectEntity project, WorkItemLinksBatch workItemLinksBatch)
    {
        var features = await _dbContext.Features
            .Include(_ => _.UserStories)
            .ToListAsync();

        var userStories = await _dbContext.UserStories
            .Include(_ => _.Tickets)
            .Include(_ => _.Bugs)
            .ToListAsync();

        var tickets = await _dbContext.Tickets
            .ToListAsync();

        var bugs = await _dbContext.Bugs
            .ToListAsync();

        foreach (var workItemLink in workItemLinksBatch.Values)
        {
            if (workItemLink.LinkType != "System.LinkTypes.Hierarchy-Forward") continue;

            var (sourceFeature, sourceUserStory) = ResolveFeatureOrUserStoryWithId(features, userStories, workItemLink.SourceId);
            var (targetUserStory, targetTicket, targetBug) = ResolveUserStoryTicketOrBugWithId(userStories, tickets, bugs, workItemLink.TargetId);

            if (sourceFeature is not null && targetTicket is not null)
            {
                _logger.LogInformation("Unexpected combination of source feature {featureId} and target ticket {ticketId}.", sourceFeature.Id, targetTicket.Id);
                continue;
            }

            if (sourceFeature is not null && targetBug is not null)
            {
                _logger.LogInformation("Unexpected combination of source feature {featureId} and target bug {bugId}.", sourceFeature.Id, targetBug.Id);
                continue;
            }

            if (sourceUserStory is not null && targetUserStory is not null)
            {
                _logger.LogInformation("Unexpected combination of source user story {sourceId} and target user story {targetId}.", sourceUserStory.Id, targetUserStory.Id);
                continue;
            }

            if ((sourceFeature is null && sourceUserStory is null) ||
                (targetUserStory is null && targetTicket is null && targetBug is null))
            {
                _logger.LogWarning("No source or target work items found.");
                continue;
            }

            if (workItemLink.IsActive)
            {
                if (sourceFeature is not null)
                {
                    if (targetUserStory is null)
                    {
                        _logger.LogInformation("Expected target user story for source feature {featureId}.", sourceFeature.Id);
                        continue;
                    }

                    sourceFeature.UserStories.Add(targetUserStory);
                }
                else
                {
                    if (targetTicket is not null)
                    {
                        sourceUserStory!.Tickets.Add(targetTicket);
                    }
                    else if (targetBug is not null)
                    {
                        sourceUserStory!.Bugs.Add(targetBug);
                    }
                    else
                    {
                        _logger.LogInformation("Expected target ticket or bug for source user story {userStoryId}.", sourceUserStory!.Id);
                        continue;
                    }
                }
            }
            else
            {
                if (sourceFeature is not null)
                {
                    if (targetUserStory is null)
                    {
                        _logger.LogInformation("Expected target user story for source feature {featureId}.", sourceFeature.Id);
                        continue;
                    }

                    sourceFeature.UserStories.Remove(targetUserStory);
                }
                else
                {
                    if (targetTicket is not null)
                    {
                        sourceUserStory!.Tickets.Remove(targetTicket);
                    }
                    else if (targetBug is not null)
                    {
                        sourceUserStory!.Bugs.Remove(targetBug);
                    }
                    else
                    {
                        _logger.LogInformation("Expected target ticket for source user story {userStoryId}.", sourceUserStory!.Id);
                        continue;
                    }

                }
            }
        }

        _logger.LogInformation("Setting continuation token for project {projectName} to '{continuationToken}'.", project.DevOpsProjectName, workItemLinksBatch.ContinuationToken);

        project.LinksContinuationToken = workItemLinksBatch.ContinuationToken;

        await _dbContext.SaveChangesAsync();
    }

    private (FeatureEntity?, UserStoryEntity?) ResolveFeatureOrUserStoryWithId(
        List<FeatureEntity> features,
        List<UserStoryEntity> userStories,
        int id)
    {
        var feature = features.FirstOrDefault(_ => _.DevOpsId == id);

        if (feature is not null) return (feature, null);


        var userStory = userStories.FirstOrDefault(_ => _.DevOpsId == id);

        if (userStory is not null) return (null, userStory);

        _logger.LogInformation("Failed to find feature or user story with id {id}.", id);

        return (null, null);
    }

    private (UserStoryEntity? Feature, TicketEntity? UserStory, BugEntity? Bug) ResolveUserStoryTicketOrBugWithId(
        List<UserStoryEntity> userStories,
        List<TicketEntity> tickets,
        List<BugEntity> bugs,
        int id)
    {
        var userStory = userStories.FirstOrDefault(_ => _.DevOpsId == id);

        if (userStory is not null) return (userStory, null, null);

        var ticket = tickets.FirstOrDefault(_ => _.DevOpsId == id);

        if (ticket is not null) return (null, ticket, null);

        var bug = bugs.FirstOrDefault(_ => _.DevOpsId == id);

        if (bug is not null) return (null, null, bug);

        _logger.LogInformation("Failed to find user story or ticket with id {id}.", id);

        return (null, null, null);
    }
}
