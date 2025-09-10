using Autorovers.Application.Abstractions.Messaging;

namespace Autorovers.Application.Todos.GetById;

public sealed record GetTodoByIdQuery(Guid TodoItemId) : IQuery<TodoResponse>;
