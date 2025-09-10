using Autorovers.Common;

namespace Autorovers.Domain.Todos;

public sealed class TodoItemDeletedDomainEvent : IDomainEvent
{
    public Guid TodoId { get; }
    public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;

    public TodoItemDeletedDomainEvent(Guid todoId)
    {
        TodoId = todoId;
    }
}
