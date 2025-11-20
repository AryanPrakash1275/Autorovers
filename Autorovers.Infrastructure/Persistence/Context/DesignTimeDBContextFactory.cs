using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Autorovers.Infrastructure.Persistence.Context
{
    public sealed class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AutoroversDbContext>
    {
        public AutoroversDbContext CreateDbContext(string[] args)
        {
            var cfg = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .AddJsonFile("appsettings.Development.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var conn = cfg.GetConnectionString("DefaultConnection");
            if (string.IsNullOrWhiteSpace(conn))
                throw new InvalidOperationException("ConnectionStrings:DefaultConnection is missing.");

            var options = new DbContextOptionsBuilder<AutoroversDbContext>()
                .UseSqlServer(conn, sql =>
                {
                    sql.MigrationsAssembly(typeof(AutoroversDbContext).Assembly.FullName);
                    sql.EnableRetryOnFailure();
                })
                .Options;

            return new AutoroversDbContext(options);
        }
    }
}
