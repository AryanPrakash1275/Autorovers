using Autorovers.Application.Abstractions.Authentication;
using Autorovers.Application.Abstractions.Data;
using Autorovers.Application.Abstractions.Messaging;
using Autorovers.Domain.Todos;
using Microsoft.EntityFrameworkCore;
using Autorovers.Common;

namespace Autorovers.Application.Todos.Delete;

internal sealed class DeleteTodoCommandHandler(IApplicationDbContext context, IUserContext userContext)
    : ICommandHandler<DeleteTodoCommand>
{
    public async Task<Result> Handle(DeleteTodoCommand command, CancellationToken cancellationToken)
    {
        TodoItem? todoItem = await context.TodoItems
            .SingleOrDefaultAsync(t => t.Id == command.TodoItemId && t.UserId == userContext.UserId, cancellationToken);

        if (todoItem is null)
        {
            return Result.Failure(TodoItemErrors.NotFound);
        }

        context.TodoItems.Remove(todoItem);

        todoItem.Raise(new TodoItemDeletedDomainEvent(todoItem.Id));

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
