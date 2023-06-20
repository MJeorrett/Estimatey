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
    private readonly IDateTimeService _dateTimeService;

    private readonly List<int> _excludedFloatPersonIds;

    public ListLoggedTimeByDeveloperQueryHandler(
        IFloatClient floatClient,
        IApplicationDbContext dbContext,
        ILogger<ListLoggedTimeByDeveloperQueryHandler> logger,
        IOptions<GlobalOptions> options,
        IDateTimeService dateTimeService)
    {
        _floatClient = floatClient;
        _dbContext = dbContext;
        _logger = logger;

        _excludedFloatPersonIds = options.Value.ExcludedFloatPersonIds;
        _dateTimeService = dateTimeService;
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
        var historicLoggedTime = await _dbContext.LoggedTime
            .Where(_ => !_excludedFloatPersonIds.Contains(_.FloatPerson.FloatId))
            .Select(_ => new LoggedTimeDto()
            {
                Id = _.FloatId,
                PersonId = _.FloatPerson.FloatId,
                Date = _.Date.ToString("yyyy-MM-dd"),
                Hours = _.Hours,
                Locked = _.Locked,
                LockedDate = _.LockedDate == null ? null :_.LockedDate.Value.ToString("yyyy-MM-dd"),
            })
            .ToListAsync();

        var recentLoggedTime = await _floatClient.GetLoggedTime(
            project.FloatId,
            project.LoggedTimeHasBeenSyncedUntil?.AddDays(1),
            DateOnly.FromDateTime(_dateTimeService.Now));

        return recentLoggedTime
            .Where(loggedTime =>
                !_excludedFloatPersonIds.Contains(loggedTime.PersonId))
            .Concat(historicLoggedTime)
            .OrderBy(_ => _.Date)
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

            await _floatClient.SyncPeople();
        }

        return await _dbContext.FloatPeople
            .Where(personEntity => personIds.Contains(personEntity.FloatId))
            .ToListAsync(cancellationToken);
    }
}
