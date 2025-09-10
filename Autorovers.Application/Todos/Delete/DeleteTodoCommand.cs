using Autorovers.Application.Abstractions.Messaging;

namespace Autorovers.Application.Todos.Delete;

public sealed record DeleteTodoCommand(Guid TodoItemId) : ICommand;
