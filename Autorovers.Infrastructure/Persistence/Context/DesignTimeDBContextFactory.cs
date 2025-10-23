using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Autorovers.Infrastructure.Persistence.Context;

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

        var conn = cfg.GetConnectionString("DefaultConnection")
                 ?? "Host=localhost;Database=autorovers;Username=postgres;Password=postgres";

        var options = new DbContextOptionsBuilder<AutoroversDbContext>()
            .UseNpgsql(conn)
            .Options;

        // ✅ DbContext ctor now takes ONLY options.
        return new AutoroversDbContext(options);
    }
}
