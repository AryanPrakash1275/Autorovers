using Autorovers.Application.Abstractions.DomainEvents;
using Autorovers.Application.Abstractions.Persistence;
using Autorovers.Infrastructure.Persistence.Context;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly AutoroversDbContext _db;
    private readonly IDomainEventsDispatcher _dispatcher;

    public UnitOfWork(AutoroversDbContext db, IDomainEventsDispatcher dispatcher)
    { _db = db; _dispatcher = dispatcher; }

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        var rows = await _db.SaveChangesAsync(ct);
        await _dispatcher.DispatchAsync(ct);
        return rows;
    }
}
