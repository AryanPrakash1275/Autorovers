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
            // ============================================================
            // 1) Determine the correct database connection string
            // ============================================================

            // First try Render / Postgres env var: Database:ConnectionString
            var conn = cfg["Database:ConnectionString"];

            // Fallback to development ConnectionStrings:DefaultConnection (SQL Server)
            if (string.IsNullOrWhiteSpace(conn))
            {
                conn = cfg.GetConnectionString("DefaultConnection");
            }

            if (string.IsNullOrWhiteSpace(conn))
            {
                throw new InvalidOperationException(
                    "No database connection string configured. Expected Database:ConnectionString or ConnectionStrings:DefaultConnection.");
            }

            // ============================================================
            // 2) Register EF Core DbContext using Npgsql (Postgres)
            // ============================================================
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

            // ============================================================
            // 3) Other infrastructure services
            // ============================================================
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
