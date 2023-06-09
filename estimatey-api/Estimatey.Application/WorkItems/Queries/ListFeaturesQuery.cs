﻿using Estimatey.Application.Common.AppRequests;
using Estimatey.Application.Common.Interfaces;
using Estimatey.Application.WorkItems.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Estimatey.Application.WorkItems.Queries;

public record ListFeaturesQuery
{
    public int ProjectId { get; init; }
}

public class ListFeaturesQueryHandler : IRequestHandler<ListFeaturesQuery, List<FeatureSummaryDto>>
{
    private readonly IApplicationDbContext _dbContext;

    public ListFeaturesQueryHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<AppResponse<List<FeatureSummaryDto>>> Handle(
        ListFeaturesQuery query,
        CancellationToken cancellationToken)
    {
        var featureSummaries = await _dbContext.Features
            .Where(_ => _.ProjectId == query.ProjectId)
            .Include(_ => _.UserStories)
                .ThenInclude(_ => _.Tickets)
            .Select(entity => FeatureSummaryDto.MapFromEntity(entity))
            .ToListAsync(cancellationToken);

        return new(200, featureSummaries.OrderBy(_ => _.SortOrder).ToList());
    }
}
