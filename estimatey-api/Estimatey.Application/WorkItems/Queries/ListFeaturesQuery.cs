using Estimatey.Application.Common.AppRequests;
using Estimatey.Application.Common.Interfaces;
using Estimatey.Application.WorkItems.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Estimatey.Application.WorkItems.Queries;

public record ListFeaturesQuery
{
    public int ProjectId { get; init; }
}

public class ListFeaturesQueryHandler : IRequestHandler<ListFeaturesQuery, List<FeatureSummary>>
{
    private readonly IApplicationDbContext _dbContext;

    public ListFeaturesQueryHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<AppResponse<List<FeatureSummary>>> Handle(
        ListFeaturesQuery query,
        CancellationToken cancellationToken)
    {
        var featureSummaries = await _dbContext.Features
            .Include(_ => _.UserStories)
                .ThenInclude(_ => _.Tickets)
            .Select(entity => FeatureSummary.MapFromEntity(entity))
            .ToListAsync(cancellationToken);

        return new(200, featureSummaries.OrderBy(_ => _.SortOrder).ToList());
    }
}
