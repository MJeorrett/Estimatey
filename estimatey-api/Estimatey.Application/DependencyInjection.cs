using Estimatey.Application.Projects.Commands.Create;
using Estimatey.Application.Common.AppRequests;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace Estimatey.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        AddFluentValidation(services);
        AddRequestHandlers(services);

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
