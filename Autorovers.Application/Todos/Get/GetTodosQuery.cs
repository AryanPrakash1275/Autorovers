using Autorovers.Application.Abstractions.Messaging;

namespace Autorovers.Application.Todos.Get;

public sealed record GetTodosQuery(Guid UserId) : IQuery<List<TodoResponse>>;
