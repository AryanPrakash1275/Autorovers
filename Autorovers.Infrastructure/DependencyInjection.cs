using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Autorovers.Infrastructure.Persistence.Context;

// Keep these only if the interfaces/classes actually exist and compile:
using Autorovers.Application.Abstractions.Data;
using Autorovers.Application.Abstractions.DomainEvents;
using Autorovers.Infrastructure.DomainEvents;

namespace Autorovers.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, IConfiguration cfg)
    {
        // Use the same key as in appsettings.json of API
        var conn = cfg.GetConnectionString("DefaultConnection");
        services.AddDbContext<AutoRoversDbContext>(opt => opt.UseNpgsql(conn));

        // Only keep this if your AutoRoversDbContext *implements* IApplicationDbContext
        // (recommended — see interface sample below)
        services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<AutoRoversDbContext>());

        // keep this if both the interface and class exist and compile
        // Otherwise, comment it out for now.
        services.AddScoped<IDomainEventsDispatcher, DomainEventsDispatcher>();

        services.AddScoped<
        Autorovers.Application.Abstractions.DomainEvents.IDomainEventsDispatcher,
        Autorovers.Infrastructure.DomainEvents.DomainEventsDispatcher>();


        services.AddHttpContextAccessor();

        return services;
    }
}
