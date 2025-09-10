namespace Autorovers.Common;

public abstract class Entity
{
    public Guid Id { get; protected set; }

    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void Raise(IDomainEvent @event) => _domainEvents.Add(@event);

    public void ClearDomainEvents() => _domainEvents.Clear();
}
