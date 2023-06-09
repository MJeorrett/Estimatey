using Estimatey.Core.Entities;
using Estimatey.Infrastructure.DevOps.Models;
using Estimatey.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Estimatey.Infrastructure.DevOps;

internal class LinksSynchronizationService
{
    public readonly ILogger _logger;

    public readonly DevOpsClient _devOpsClient;

    public readonly ApplicationDbContext _dbContext;

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
        _logger.LogInformation("Syncing links for project {projectName}.", project.DevOpsProjectName);

        var workItemLinksDto = await _devOpsClient.GetWorkItemLinksBatch(project.DevOpsProjectName, project.LinksContinuationToken)
;
        await ProcessWorkItemLinks(project, workItemLinksDto);

        _logger.LogInformation("Successfully synced links for project {projectName}.", project.DevOpsProjectName);

        if (!workItemLinksDto.IsLastBatch)
        {
            await SyncProject(project);
        }
    }

    private async Task ProcessWorkItemLinks(ProjectEntity project, WorkItemLinksDto workItemLinksDto)
    {
        var features = await _dbContext.Features
            .Include(_ => _.UserStories)
            .ToListAsync();

        var userStories = await _dbContext.UserStories
            .Include(_ => _.Tickets)
            .ToListAsync();

        var tickets = await _dbContext.Tickets
            .ToListAsync();

        foreach (var workItemLink in workItemLinksDto.Values)
        {
            if (workItemLink.LinkType != "System.LinkTypes.Hierarchy-Forward") continue;

            var (sourceFeature, sourceUserStory) = ResolveFeatureOrUserStoryWithId(features, userStories, workItemLink.SourceId);
            var (targetUserStory, targetTicket) = ResolveUserStoryOrTicketWithId(userStories, tickets, workItemLink.TargetId);

            if (sourceFeature is not null && targetTicket is not null)
            {
                _logger.LogInformation("Unexpected combination of source feature {featureId} and target ticket {ticketId}.", sourceFeature.Id, targetTicket.Id);
                continue;
            }

            if (sourceUserStory is not null && targetUserStory is not null)
            {
                _logger.LogInformation("Unexpected combination of source user story {sourceId} and target user story {targetId}.", sourceUserStory.Id, targetUserStory.Id);
                continue;
            }

            if ((sourceFeature is null && sourceUserStory is null) ||
                (targetUserStory is null && targetTicket is null))
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
                    if (targetTicket is null)
                    {
                        _logger.LogInformation("Expected target ticket for source user story {userStoryId}.", sourceUserStory!.Id);
                        continue;
                    }

                    sourceUserStory!.Tickets.Add(targetTicket);
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
                    if (targetTicket is null)
                    {
                        _logger.LogInformation("Expected target ticket for source user story {userStoryId}.", sourceUserStory!.Id);
                        continue;
                    }

                    sourceUserStory!.Tickets.Remove(targetTicket);
                }
            }
        }

        _logger.LogInformation("Setting continuation token for project {projectName} to '{continuationToken}'.", project.DevOpsProjectName, workItemLinksDto.ContinuationToken);

        project.LinksContinuationToken = workItemLinksDto.ContinuationToken;

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

    private (UserStoryEntity? Feature, TicketEntity? UserStory) ResolveUserStoryOrTicketWithId(
        List<UserStoryEntity> userStories,
        List<TicketEntity> tickets,
        int id)
    {
        var userStory = userStories.FirstOrDefault(_ => _.DevOpsId == id);

        if (userStory is not null) return (userStory, null);

        var ticket = tickets.FirstOrDefault(_ => _.DevOpsId == id);

        if (ticket is not null) return (null, ticket);

        _logger.LogInformation("Failed to find user story or ticket with id {id}.", id);

        return (null, null);
    }
}
