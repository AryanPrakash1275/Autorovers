using Autorovers.Application.Abstractions.Messaging;

namespace Autorovers.Application.Todos.Complete;

public sealed record CompleteTodoCommand(Guid TodoItemId) : ICommand;
