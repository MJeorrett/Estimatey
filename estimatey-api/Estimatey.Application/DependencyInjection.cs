using Estimatey.Application.Projects.Commands.Create;
using Estimatey.Application.Common.AppRequests;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Estimatey.Application.Common.Configuration;

namespace Estimatey.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        AddFluentValidation(services);
        AddRequestHandlers(services);

        services.AddOptions<GlobalOptions>()
            .Bind(configuration.GetSection("GlobalOptions"))
            .ValidateOnStart();

        return services;
    }
    private static void AddFluentValidation(IServiceCollection services)
    {
        services
            .AddFluentValidationAutoValidation(config =>
            {
                config.DisableDataAnnotationsValidation = true;
            })
            .AddValidatorsFromAssemblyContaining<CreateProjectCommandValidator>();
    }

    private static void AddRequestHandlers(IServiceCollection services)
    {
        services.Scan(scan =>
        {
            scan.FromAssemblyOf<CreateProjectCommandHandler>()
                .AddClasses(classes => classes.AssignableTo(typeof(IRequestHandler<,>)))
                .AsSelf()
                .WithScopedLifetime();

            scan.FromAssemblyOf<CreateProjectCommandHandler>()
                .AddClasses(classes => classes.AssignableTo(typeof(IRequestHandler<>)))
                .AsSelf()
                .WithScopedLifetime();
        });
    }
}
