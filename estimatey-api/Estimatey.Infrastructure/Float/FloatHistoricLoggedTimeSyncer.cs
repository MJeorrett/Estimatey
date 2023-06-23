using Estimatey.Application.Common.Interfaces;
using Estimatey.Core.Entities;
using Estimatey.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;

namespace Estimatey.Infrastructure.Float;

public class FloatHistoricLoggedTimeSyncer
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger _logger;
    private readonly IDateTimeService _dateTimeService;
    private readonly IFloatClient _floatClient;

    private readonly int _persistLoggedTimeBeforeDaysAgo;

    private static readonly ConcurrentDictionary<int, SemaphoreSlim> _semaphores = new();

    public FloatHistoricLoggedTimeSyncer(
        IOptions<FloatOptions> options,
        ApplicationDbContext dbContext,
        ILogger<FloatHistoricLoggedTimeSyncer> logger,
        IDateTimeService dateTimeService,
        IFloatClient floatClient)
    {
        _dbContext = dbContext;

        _persistLoggedTimeBeforeDaysAgo = options.Value.PersistLoggedTimeBeforeDaysAgo;
        _logger = logger;
        _dateTimeService = dateTimeService;
        _floatClient = floatClient;
    }

    public async Task SyncHistoricLoggedTime()
    {
        await _floatClient.SyncPeople();

        var projects = await _dbContext.Projects.ToListAsync();
        var people = await _dbContext.FloatPeople.ToListAsync();

        _logger.LogInformation("Syncing historic logged time for {projectsCount} projects.", projects.Count);

        foreach (var project in projects)
        {
            await SyncProject(project, people);
        }
    }

    private async Task SyncProject(ProjectEntity project, List<FloatPersonEntity> people)
    {
        var semaphore = _semaphores.GetOrAdd(project.Id, _ => new SemaphoreSlim(1, 1));
        await semaphore.WaitAsync();

        try
        {
            var syncLoggedTimeUntil = _dateTimeService.NowDateOnly.AddDays(-_persistLoggedTimeBeforeDaysAgo - 1);

            if (project.LoggedTimeHasBeenSyncedUntil is not null &&
                project.LoggedTimeHasBeenSyncedUntil >= syncLoggedTimeUntil)
            {
                _logger.LogInformation("Historic logged time is up to date so no need to sync.");
                return;
            }

            DateOnly? syncLoggedTimeFrom = project.LoggedTimeHasBeenSyncedUntil.HasValue ?
                project.LoggedTimeHasBeenSyncedUntil.Value.AddDays(1) :
                null;

            _logger.LogDebug("Syncing historic logged time for project {projectId} between {startDate} and {endDate}.", project.Id, syncLoggedTimeFrom, syncLoggedTimeUntil);

            var historicLoggedTime = await _floatClient.GetLoggedTime(project.FloatId, syncLoggedTimeFrom, syncLoggedTimeUntil);

            var loggedTimeEntities = historicLoggedTime.Select(_ => new LoggedTimeEntity
            {
                FloatId = _.Id,
                FloatPersonId = people.First(person => person.FloatId == _.PersonId).Id,
                Date = DateOnly.Parse(_.Date),
                Hours = _.Hours,
                Locked = _.Locked,
                LockedDate = _.LockedDate is null ? null : DateOnly.Parse(_.LockedDate),
            });

            _logger.LogDebug("Persisting {loggedTimeEntriesCount} logged time entries.", loggedTimeEntities.Count());

            project.LoggedTimeHasBeenSyncedUntil = syncLoggedTimeUntil;

            _dbContext.LoggedTime.AddRange(loggedTimeEntities);
            await _dbContext.SaveChangesAsync();

            _logger.LogDebug("Successfully synced historic logged time for project {projectId} between {startDate} and {endDate}.", project.Id, syncLoggedTimeFrom, syncLoggedTimeUntil);
        }
        finally
        {
            semaphore.Release();
        }
    }
}
