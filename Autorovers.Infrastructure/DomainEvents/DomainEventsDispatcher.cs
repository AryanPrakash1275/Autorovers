using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using Autorovers.Common; // Entity, IDomainEvent
using Autorovers.Application.Abstractions.DomainEvents; // IDomainEventsDispatcher, IDomainEventHandler<>
using Autorovers.Infrastructure.Persistence.Context; // AutoRoversDbContext

namespace Autorovers.Infrastructure.DomainEvents;

public sealed class DomainEventsDispatcher(IServiceProvider serviceProvider,
                                           AutoRoversDbContext db)
    : IDomainEventsDispatcher
{
    private static readonly ConcurrentDictionary<Type, Type> HandlerTypeDictionary = new();
    private static readonly ConcurrentDictionary<Type, Type> WrapperTypeDictionary = new();

    // ? This matches your interface exactly
    public async Task DispatchEventsAsync(CancellationToken cancellationToken = default)
    {
        // 1) collect events from tracked entities
        var domainEntities = db.ChangeTracker
            .Entries<Entity>()
            .Where(e => e.Entity.DomainEvents.Any())
            .Select(e => e.Entity)
            .ToList();

        var eventsToPublish = domainEntities
            .SelectMany(e => e.DomainEvents)
            .ToList();

        // 2) clear them on entities so they aren't re-published
        domainEntities.ForEach(e => e.ClearDomainEvents());

        // 3) delegate to your existing method
        await DispatchAsync(eventsToPublish, cancellationToken);
    }

    // ? keep your existing helper
    public async Task DispatchAsync(IEnumerable<IDomainEvent> domainEvents,
                                    CancellationToken cancellationToken = default)
    {
        foreach (IDomainEvent domainEvent in domainEvents)
        {
            using IServiceScope scope = serviceProvider.CreateScope();

            Type domainEventType = domainEvent.GetType();
            Type handlerType = HandlerTypeDictionary.GetOrAdd(
                domainEventType,
                et => typeof(IDomainEventHandler<>).MakeGenericType(et));

            IEnumerable<object?> handlers = scope.ServiceProvider.GetServices(handlerType);

            foreach (object? handler in handlers)
            {
                if (handler is null) continue;

                var handlerWrapper = HandlerWrapper.Create(handler, domainEventType);
                await handlerWrapper.Handle(domainEvent, cancellationToken);
            }
        }
    }

    private abstract class HandlerWrapper
    {
        public abstract Task Handle(IDomainEvent domainEvent, CancellationToken cancellationToken);

        public static HandlerWrapper Create(object handler, Type domainEventType)
        {
            Type wrapperType = WrapperTypeDictionary.GetOrAdd(
                domainEventType,
                et => typeof(HandlerWrapper<>).MakeGenericType(et));

            return (HandlerWrapper)Activator.CreateInstance(wrapperType, handler)!;
        }
    }

    private sealed class HandlerWrapper<T>(object handler) : HandlerWrapper where T : IDomainEvent
    {
        private readonly IDomainEventHandler<T> _handler = (IDomainEventHandler<T>)handler;

        public override Task Handle(IDomainEvent domainEvent, CancellationToken cancellationToken)
            => _handler.Handle((T)domainEvent, cancellationToken);
    }
}
