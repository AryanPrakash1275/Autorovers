using Autorovers.Common;

namespace Autorovers.Domain.Todos;

public sealed class TodoItemCompletedDomainEvent : IDomainEvent
{
    public Guid TodoId { get; }
    public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;

    public TodoItemCompletedDomainEvent(Guid todoId)
    {
        TodoId = todoId;
    }
}
