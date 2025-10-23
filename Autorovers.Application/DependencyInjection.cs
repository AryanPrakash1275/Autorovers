using System.Linq;
using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Autorovers.Application.Abstractions.Messaging;
using Autorovers.Application.Abstractions.Behaviors;

namespace Autorovers.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        services.Scan(scan => scan
            .FromAssemblies(assembly)
            .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<,>)))
                .AsImplementedInterfaces().WithScopedLifetime()
            .AddClasses(c => c.AssignableTo(typeof(IQueryHandler<,>)))
                .AsImplementedInterfaces().WithScopedLifetime()
        );

        services.AddValidatorsFromAssembly(assembly, includeInternalTypes: true);

        // Decorate commands only if any are registered
        if (services.Any(d => d.ServiceType.IsGenericType &&
                              d.ServiceType.GetGenericTypeDefinition() == typeof(ICommandHandler<,>)))
        {
            services.Decorate(typeof(ICommandHandler<,>), typeof(ValidationDecorator.CommandHandler<,>));
        }

        // ⚠️ Skip query decorator entirely until you actually have queries
        // If you want it later, add the same guard as above and re-enable.

        return services;
    }
}
