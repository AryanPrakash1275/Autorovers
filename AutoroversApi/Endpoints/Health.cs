using Autorovers.Infrastructure.Persistence.Context;

namespace AutoroversApi.Endpoints;

public static class HealthEndpoint
{
    public static IEndpointRouteBuilder MapHealth(this IEndpointRouteBuilder app)
    {
        // Liveness: app is running (no DB)
        app.MapGet("/health", () =>
        {
            return Results.Ok(new { ok = true, service = "autorovers-api" });
        }).WithTags("Health");

        // Readiness: DB reachable
        app.MapGet("/health/db", async (AutoroversDbContext db) =>
        {
            var ok = await db.Database.CanConnectAsync();
            return Results.Ok(new { ok, provider = db.Database.ProviderName });
        }).WithTags("Health");

        return app;
    }
}
