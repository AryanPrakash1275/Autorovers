using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Autorovers.Infrastructure.Persistence.Context;

// Abstractions
using Autorovers.Application.Abstractions.DomainEvents;
using Autorovers.Application.Abstractions.Authentication;
using Autorovers.Application.Abstractions.Persistence;
using Autorovers.Application.Vehicles;
using Autorovers.Common;

// Implementations
using Autorovers.Infrastructure.Authorization;
using Autorovers.Infrastructure.Persistence;
using Autorovers.Infrastructure.Time;

namespace Autorovers.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration cfg)
        {
            var conn = cfg.GetConnectionString("DefaultConnection");
            if (string.IsNullOrWhiteSpace(conn))
                throw new InvalidOperationException("ConnectionStrings:DefaultConnection is missing.");

            // ===== POSTGRES VERSION FOR deploy-postgres BRANCH =====
            services.AddDbContext<AutoroversDbContext>(opt =>
                opt.UseNpgsql(
                    conn,
                    npgsql =>
                    {
                        npgsql.MigrationsAssembly(typeof(AutoroversDbContext).Assembly.FullName);
                        npgsql.EnableRetryOnFailure(
                            maxRetryCount: 5,
                            maxRetryDelay: TimeSpan.FromSeconds(10),
                            errorCodesToAdd: null);
                    }));

            services.AddScoped<Autorovers.Application.Abstractions.Data.IApplicationDbContext>(
                sp => sp.GetRequiredService<AutoroversDbContext>());

            services.AddHttpContextAccessor();
            services.AddScoped<IDateTimeProvider, SystemDateTimeProvider>();
            services.AddScoped<IUserContext, UserContext>();
            services.AddScoped<IPasswordHasher, BcryptPasswordHasher>();

            services.AddScoped<IDomainEventsDispatcher, DomainEventsDispatcher>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IVehicleService, VehicleService>();

            return services;
        }
    }
}
