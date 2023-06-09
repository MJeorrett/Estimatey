using Estimatey.Application.Common.Interfaces;
using Estimatey.Infrastructure.DateTimes;
using Estimatey.Infrastructure.DevOps;
using Estimatey.Infrastructure.Float;
using Estimatey.Infrastructure.Persistence;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Estimatey.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPersistence(configuration);

        services.AddSingleton<IDateTimeService, DateTimeService>();
        services.AddScoped<WorkItemSynchronizationService>();
        services.AddScoped<LinksSynchronizationService>();
        services.AddScoped<FloatHistoricLoggedTimeSyncer>();

        services.AddHostedService<ProjectsSynchronizationService>();
        services.AddHttpClient<DevOpsClient>();
        services.AddHttpClient<FloatClient>();

        services.AddScoped<IFloatClient>(provider => provider.GetRequiredService<FloatClient>());

        services.AddOptions<DevOpsOptions>()
            .Bind(configuration.GetSection("DevOpsOptions"))
            .ValidateOnStart();
        
        services.AddOptions<FloatOptions>()
            .Bind(configuration.GetSection("FloatOptions"))
            .ValidateOnStart();

        services.AddHangfire(config =>
            config.UseSqlServerStorage(configuration.GetConnectionString("SqlServer")));

        services.AddHangfireServer();

        return services;
    }

    private static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("SqlServer"),
                builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
    }
}
