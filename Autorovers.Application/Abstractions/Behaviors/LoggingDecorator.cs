using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Autorovers.Application.Abstractions.Messaging;
using Microsoft.Extensions.Logging;
using Autorovers.Common;

namespace Autorovers.Application.Abstractions.Behaviors;

public static class LoggingDecorator
{
    public sealed class CommandHandler<TCommand, TResponse>(
        ICommandHandler<TCommand, TResponse> inner,
        ILogger<CommandHandler<TCommand, TResponse>> logger)
        : ICommandHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
    {
        public async Task<Result<TResponse>> Handle(TCommand command, CancellationToken ct)
        {
            var name = typeof(TCommand).Name;
            using var scope = logger.BeginScope(new Dictionary<string, object?> { ["Command"] = name });

            logger.LogInformation("Processing command {Command}", name);
            var result = await inner.Handle(command, ct);
            if (result.IsSuccess) logger.LogInformation("Completed command {Command}", name);
            else { using var es = logger.BeginScope(new Dictionary<string, object?> { ["Error"] = result.Error }); logger.LogError("Completed command {Command} with error", name); }
            return result;
        }
    }

    public sealed class CommandBaseHandler<TCommand>(
        ICommandHandler<TCommand> inner,
        ILogger<CommandBaseHandler<TCommand>> logger)
        : ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        public async Task<Result> Handle(TCommand command, CancellationToken ct)
        {
            var name = typeof(TCommand).Name;
            using var scope = logger.BeginScope(new Dictionary<string, object?> { ["Command"] = name });

            logger.LogInformation("Processing command {Command}", name);
            var result = await inner.Handle(command, ct);
            if (result.IsSuccess) logger.LogInformation("Completed command {Command}", name);
            else { using var es = logger.BeginScope(new Dictionary<string, object?> { ["Error"] = result.Error }); logger.LogError("Completed command {Command} with error", name); }
            return result;
        }
    }

    public sealed class QueryHandler<TQuery, TResponse>(
        IQueryHandler<TQuery, TResponse> inner,
        ILogger<QueryHandler<TQuery, TResponse>> logger)
        : IQueryHandler<TQuery, TResponse>
        where TQuery : IQuery<TResponse>
    {
        public async Task<Result<TResponse>> Handle(TQuery query, CancellationToken ct)
        {
            var name = typeof(TQuery).Name;
            using var scope = logger.BeginScope(new Dictionary<string, object?> { ["Query"] = name });

            logger.LogInformation("Processing query {Query}", name);
            var result = await inner.Handle(query, ct);
            if (result.IsSuccess) logger.LogInformation("Completed query {Query}", name);
            else { using var es = logger.BeginScope(new Dictionary<string, object?> { ["Error"] = result.Error }); logger.LogError("Completed query {Query} with error", name); }
            return result;
        }
    }
}
