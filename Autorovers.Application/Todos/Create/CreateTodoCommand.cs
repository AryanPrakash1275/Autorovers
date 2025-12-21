using Autorovers.Application.Abstractions.Messaging;
using Autorovers.Domain.Todos;

namespace Autorovers.Application.Todos.Create;

public sealed class CreateTodoCommand : ICommand<Guid>
{
    public Guid UserId { get; set; }
    public required string Description { get; init; }
    public DateTime? DueDate { get; set; }
    public List<string> Labels { get; set; } = [];
    public Priority Priority { get; set; }
}
