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

namespace Autorovers.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration cfg)
        {
            var conn = cfg.GetConnectionString("DefaultConnection");
            if (string.IsNullOrWhiteSpace(conn))
                throw new InvalidOperationException("ConnectionStrings:DefaultConnection is missing.");

            services.AddDbContext<AutoroversDbContext>(opt =>
                opt.UseSqlServer(
                    conn,
                    sql =>
                    {
                        sql.MigrationsAssembly(typeof(AutoroversDbContext).Assembly.FullName);
                        sql.EnableRetryOnFailure(maxRetryCount: 5,
                                                 maxRetryDelay: TimeSpan.FromSeconds(10),
                                                 errorNumbersToAdd: null);
                    }));

            services.AddScoped<Autorovers.Application.Abstractions.Data.IApplicationDbContext>(
                sp => sp.GetRequiredService<AutoroversDbContext>());

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
