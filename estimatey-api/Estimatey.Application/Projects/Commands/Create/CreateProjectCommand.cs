using Estimatey.Application.Common.AppRequests;
using Estimatey.Application.Common.Interfaces;
using Estimatey.Core.Entities;

namespace Estimatey.Application.Projects.Commands.Create;

public class CreateProjectCommand
{
    public string DevOpsName { get; init; } = null!;
}

public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, int>
{
    private readonly IApplicationDbContext _dbContext;

    public CreateProjectCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<AppResponse<int>> Handle(
        CreateProjectCommand command,
        CancellationToken cancellationToken)
    {
        var entity = new ProjectEntity
        {
            DevOpsProjectName = command.DevOpsName,
        };

        _dbContext.Projects.Add(entity);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new(201, entity.Id);
    }
}
