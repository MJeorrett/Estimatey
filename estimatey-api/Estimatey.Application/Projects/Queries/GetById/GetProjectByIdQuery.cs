using Estimatey.Application.Projects.Dtos;
using Estimatey.Application.Common.AppRequests;
using Estimatey.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Estimatey.Application.Projects.Queries.GetById;

public record GetProjectByIdQuery
{
    public int ProjectId { get; init; }
}

public class GetProjectByIdQueryHandler : IRequestHandler<GetProjectByIdQuery, ProjectDto>
{
    private readonly IApplicationDbContext _dbContext;

    public GetProjectByIdQueryHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<AppResponse<ProjectDto>> Handle(
        GetProjectByIdQuery query,
        CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Projects
            .FirstOrDefaultAsync(_ => _.Id == query.ProjectId, cancellationToken);

        if (entity == null)
        {
            return new(404);
        }

        return new(200, ProjectDto.MapFromEntity(entity));
    }
}
