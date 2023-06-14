using Estimatey.Application.Common.AppRequests;
using Estimatey.Application.Common.Configuration;
using Estimatey.Application.Common.Interfaces;
using Estimatey.Application.Common.Models;
using Estimatey.Application.DeveloperTime.Dtos;
using Estimatey.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Estimatey.Application.DeveloperTime.Queries.ListLoggedTimeByDeveloper;

public record ListLoggedTimeByDeveloperQuery
{
    public int ProjectId { get; init; }
}

public class ListLoggedTimeByDeveloperQueryHandler : IRequestHandler<ListLoggedTimeByDeveloperQuery, List<DeveloperLoggedTime>>
{
    private readonly IFloatClient _floatClient;
    private readonly IApplicationDbContext _dbContext;
    private readonly ILogger _logger;
    private readonly List<int> _excludedFloatPersonIds;

    private static SemaphoreSlim _syncPeopleSemaphore = new SemaphoreSlim(1, 1);

    public ListLoggedTimeByDeveloperQueryHandler(
        IFloatClient floatClient,
        IApplicationDbContext dbContext,
        ILogger<ListLoggedTimeByDeveloperQueryHandler> logger,
        IOptions<GlobalOptions> options)
    {
        _floatClient = floatClient;
        _dbContext = dbContext;
        _logger = logger;

        _excludedFloatPersonIds = options.Value.ExcludedFloatPersonIds;
    }

    public async Task<AppResponse<List<DeveloperLoggedTime>>> Handle(
        ListLoggedTimeByDeveloperQuery query,
        CancellationToken cancellationToken)
    {
        var project = await _dbContext.Projects.FirstOrDefaultAsync(_ => _.Id == query.ProjectId, cancellationToken);

        if (project is null)
        {
            return new(404, null, $"No project exists with id {query.ProjectId}.");
        }

        var loggedDeveloperTime = await GetLoggedDeveloperTime(project);

        var personIds = loggedDeveloperTime.Select(_ => _.PersonId).Distinct().ToList();
        var people = await GetPeople(personIds, cancellationToken);

        var developerLoggedTime = DeveloperLoggedTime.MapFromLoggedTime(loggedDeveloperTime, people);

        return new(200, developerLoggedTime);
    }

    private async Task<List<LoggedTimeDto>> GetLoggedDeveloperTime(ProjectEntity project)
    {
        var allLoggedTime = await _floatClient.GetLoggedTime(project.FloatId);

        return allLoggedTime
            .Where(loggedTime =>
                !_excludedFloatPersonIds.Contains(loggedTime.PersonId))
            .ToList();
    }

    private async Task<List<FloatPersonEntity>> GetPeople(List<int> personIds, CancellationToken cancellationToken)
    {
        var existingPeopleFloatIds = await _dbContext.FloatPeople
            .Select(_ => _.FloatId)
            .ToListAsync(cancellationToken);

        var missingPeopleCount = personIds.Count(personId => !existingPeopleFloatIds.Contains(personId));

        if (missingPeopleCount != 0)
        {
            _logger.LogInformation("Found {missingPeopleCount} people in response from Float which don't exist in database so syncing people.", missingPeopleCount);

            await SyncPeopleWithFloat(cancellationToken);
        }

        return await _dbContext.FloatPeople
            .Where(personEntity => personIds.Contains(personEntity.FloatId))
            .ToListAsync(cancellationToken);
    }

    private async Task SyncPeopleWithFloat(CancellationToken cancellationToken)
    {
        await _syncPeopleSemaphore.WaitAsync();

        try
        {
            // Re-fetch inside the lock to make sure we have bang up to date people from db.
            var existingPeopleFloatIds = await _dbContext.FloatPeople
                .Select(_ => _.FloatId)
                .ToListAsync(cancellationToken);

            var peopleFromFloat = await _floatClient.GetPeople();

            foreach (var floatPerson in peopleFromFloat)
            {
                if (!existingPeopleFloatIds.Contains(floatPerson.Id))
                {
                    _logger.LogInformation("{personName} doesn't exist so adding them.", floatPerson.Name);

                    _dbContext.FloatPeople.Add(new()
                    {
                        Name = floatPerson.Name,
                        FloatId = floatPerson.Id,
                    });
                }
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        finally
        {
            _syncPeopleSemaphore.Release();
        }
    }
}
