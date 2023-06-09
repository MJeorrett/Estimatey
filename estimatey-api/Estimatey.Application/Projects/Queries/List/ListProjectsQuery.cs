using Estimatey.Application.Projects.Dtos;
using Estimatey.Application.Common.AppRequests;
using Estimatey.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Estimatey.Application.Projects.Queries.List;

public record ListProjectsQuery
{

}

public class ListProjectsQueryHandler : IRequestHandler<ListProjectsQuery, List<ProjectDto>>
{
    private readonly IApplicationDbContext _dbContext;

    public ListProjectsQueryHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<AppResponse<List<ProjectDto>>> Handle(
        ListProjectsQuery query,
        CancellationToken cancellationToken)
    {
        var dtos = await _dbContext.Projects
            .Select(_ => ProjectDto.MapFromEntity(_))
            .ToListAsync(cancellationToken);

        return new(200, dtos);
    }
}
