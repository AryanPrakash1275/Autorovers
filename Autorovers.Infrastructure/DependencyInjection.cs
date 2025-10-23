using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Autorovers.Infrastructure.Persistence.Context;

// Abstractions
using Autorovers.Application.Abstractions.DomainEvents;
using Autorovers.Application.Abstractions.Authentication;
using Autorovers.Application.Abstractions.Persistence;
using Autorovers.Common;

// Implementations
using Autorovers.Infrastructure.Authorization;
using Autorovers.Infrastructure.Persistence;
using Autorovers.Infrastructure.Time;

namespace Autorovers.Infrastructure // ⬅️ THIS NAMESPACE MUST MATCH
{
    public static class DependencyInjection // ⬅️ AND THIS CLASS NAME
    {
        // Extension or non-extension is fine; we’ll call it explicitly anyway.
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration cfg)
        {
            var conn = cfg.GetConnectionString("DefaultConnection")
                      ?? "Host=localhost;Database=autorovers;Username=postgres;Password=postgres";

            services.AddDbContext<AutoroversDbContext>(opt => opt.UseNpgsql(conn));
            services.AddScoped<Autorovers.Application.Abstractions.Data.IApplicationDbContext>(
            sp => sp.GetRequiredService<Autorovers.Infrastructure.Persistence.Context.AutoroversDbContext>());

            services.AddHttpContextAccessor();
            services.AddScoped<IDateTimeProvider, SystemDateTimeProvider>();
            services.AddScoped<IUserContext, UserContext>();
            services.AddScoped<IPasswordHasher, BcryptPasswordHasher>();

            services.AddScoped<IDomainEventsDispatcher, DomainEventsDispatcher>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
