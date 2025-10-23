using Autorovers.Application.Abstractions.DomainEvents;
using Autorovers.Infrastructure.Persistence.Context;

public sealed class DomainEventsDispatcher : IDomainEventsDispatcher
{
    private readonly AutoroversDbContext _db;
    public DomainEventsDispatcher(AutoroversDbContext db) => _db = db;

    public async Task DispatchAsync(CancellationToken ct = default)
    {
        var aggregates = _db.GetAggregatesWithEvents().ToList();
        var events = aggregates.SelectMany(a => a.DomainEvents).ToList();

        // TODO: publish events to handlers/mediator here

        foreach (var agg in aggregates) agg.ClearDomainEvents();
        await Task.CompletedTask;
    }
}
