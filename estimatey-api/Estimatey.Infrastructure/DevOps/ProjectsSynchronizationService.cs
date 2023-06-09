using Estimatey.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Estimatey.Infrastructure.DevOps;

internal class ProjectsSynchronizationService : BackgroundService
{
    private const int PauseBetweenSyncsMilliseconds = 8000;

    private readonly IServiceProvider _services;
    private readonly ILogger _logger;

    public ProjectsSynchronizationService(
        IServiceProvider services,
        ILogger<ProjectsSynchronizationService> logger)
    {
        _services = services;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Syncing projects.");

            await SyncAllProjects(stoppingToken);

            _logger.LogInformation("Finished syncing projects.");

            if (stoppingToken.IsCancellationRequested) return;

            Thread.Sleep(PauseBetweenSyncsMilliseconds);
        }
    }

    private async Task SyncAllProjects(CancellationToken stoppingToken)
    {
        using var scope = _services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var workItemSynchronizer = scope.ServiceProvider.GetRequiredService<WorkItemSynchronizationService>();
        var workItemLinkSynchronizer = scope.ServiceProvider.GetRequiredService<LinksSynchronizationService>();

        var projects = await dbContext.Projects.ToListAsync();

        _logger.LogInformation("Syncing {projectsCount} projects.", projects.Count);

        foreach (var project in projects)
        {
            await workItemSynchronizer.SyncProject(project);

            await workItemLinkSynchronizer.SyncProject(project);

            if (stoppingToken.IsCancellationRequested) return;
        }
    }
}
