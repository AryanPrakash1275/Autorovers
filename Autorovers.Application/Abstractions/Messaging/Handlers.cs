using System.Threading;
using System.Threading.Tasks;
using Autorovers.Common;

namespace Autorovers.Application.Abstractions.Messaging;

public interface ICommandHandler<TCommand> where TCommand : ICommand
{
    Task<Result> Handle(TCommand command, CancellationToken ct);
}

public interface ICommandHandler<TCommand, TResponse> where TCommand : ICommand<TResponse>
{
    Task<Result<TResponse>> Handle(TCommand command, CancellationToken ct);
}

public interface IQueryHandler<TQuery, TResponse> where TQuery : IQuery<TResponse>
{
    Task<Result<TResponse>> Handle(TQuery query, CancellationToken ct);
}
