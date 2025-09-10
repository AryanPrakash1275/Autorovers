using Autorovers.Application.Abstractions.Authentication;
using Autorovers.Application.Abstractions.Data;
using Autorovers.Application.Abstractions.Messaging;
using Autorovers.Domain.Todos;
using Microsoft.EntityFrameworkCore;
using Autorovers.Common;

namespace Autorovers.Application.Todos.Complete;

internal sealed class CompleteTodoCommandHandler(
    IApplicationDbContext context,
    IDateTimeProvider dateTimeProvider,
    IUserContext userContext)
    : ICommandHandler<CompleteTodoCommand>
{
    public async Task<Result> Handle(CompleteTodoCommand command, CancellationToken cancellationToken)
    {
        TodoItem? todoItem = await context.TodoItems
            .SingleOrDefaultAsync(t => t.Id == command.TodoItemId && t.UserId == userContext.UserId, cancellationToken);

        if (todoItem is null)
        {
            return Result.Failure(TodoItemErrors.NotFound);
        }

        if (todoItem.IsCompleted)
        {
            return Result.Failure(TodoItemErrors.AlreadyCompleted);
        }

        todoItem.IsCompleted = true;
        todoItem.CompletedAt = dateTimeProvider.UtcNow;

        todoItem.Raise(new TodoItemCompletedDomainEvent(todoItem.Id));

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
