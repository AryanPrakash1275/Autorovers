using Autorovers.Common;

namespace Autorovers.Domain.Todos;

public sealed class TodoItemCreatedDomainEvent : IDomainEvent
{
    public Guid TodoId { get; }
    public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;

    public TodoItemCreatedDomainEvent(Guid todoId)
    {
        TodoId = todoId;
    }
}
