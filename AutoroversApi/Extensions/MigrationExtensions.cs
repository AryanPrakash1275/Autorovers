using Autorovers.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace AutoroversApi.Extensions;

public static class MigrationExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        using AutoroversDbContext dbContext =
            scope.ServiceProvider.GetRequiredService<AutoroversDbContext>();

        dbContext.Database.Migrate();
    }
}
