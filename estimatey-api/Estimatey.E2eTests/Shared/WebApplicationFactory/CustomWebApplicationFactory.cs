using Estimatey.Application.Common.Interfaces;
using Estimatey.E2eTests.Shared.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Respawn;
using Respawn.Graph;

namespace Estimatey.E2eTests.Shared.WebApplicationFactory;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private Checkpoint _checkpoint = null!;

    public async Task ResetState()
    {
        var services = GetScopedServiceProvider();

        var configuration = services.GetRequiredService<IConfiguration>();

        var connectionString = configuration.GetConnectionString("SqlServer") ?? throw new Exception("No connection string defined.");
        await _checkpoint.Reset(connectionString);
    }

    public IServiceProvider GetScopedServiceProvider()
    {
        return Services.CreateScope().ServiceProvider;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("E2eTests");

        builder.ConfigureServices((context, services) =>
        {
            services.ReplaceServiceWithMock<IDateTimeService>();
        });

        _checkpoint = new Checkpoint
        {
            TablesToIgnore = new[] {
               new Table("__EFMigrationsHistory"),
            },
        };

        base.ConfigureWebHost(builder);
    }
}
