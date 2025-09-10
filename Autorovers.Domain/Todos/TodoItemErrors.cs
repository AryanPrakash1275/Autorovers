using Autorovers.Common;

namespace Autorovers.Domain.Todos;

public static class TodoItemErrors
{
    public static readonly Error NotFound = new("Todo.NotFound", "Todo item not found.", ErrorType.NotFound);
    public static readonly Error AlreadyCompleted = new("Todo.AlreadyCompleted", "Todo item is already completed.", ErrorType.Validation);
}
