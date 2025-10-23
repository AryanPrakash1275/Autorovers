using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Autorovers.Application.Abstractions.Messaging;
using Autorovers.Common; // Result<T>, Error, ErrorType

namespace Autorovers.Application.Abstractions.Behaviors;

public static class ValidationDecorator
{
    public sealed class CommandHandler<TCommand, TResponse> : ICommandHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
    {
        private readonly ICommandHandler<TCommand, TResponse> _inner;
        private readonly IEnumerable<IValidator<TCommand>> _validators;

        public CommandHandler(
            ICommandHandler<TCommand, TResponse> inner,
            IEnumerable<IValidator<TCommand>> validators)
        {
            _inner = inner;
            _validators = validators;
        }

        public async Task<Result<TResponse>> Handle(TCommand command, CancellationToken cancellationToken = default)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TCommand>(command);
                var results = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
                var failures = results.SelectMany(r => r.Errors).Where(f => f is not null).ToList();

                if (failures.Count > 0)
                {
                    var message = string.Join("; ", failures.Select(f => f.ErrorMessage));
                    return Result.Failure<TResponse>(new Error("validation_failed", message, ErrorType.Validation));
                }
            }

            return await _inner.Handle(command, cancellationToken);
        }
    }

    public sealed class QueryHandler<TQuery, TResponse> : IQueryHandler<TQuery, TResponse>
        where TQuery : IQuery<TResponse>
    {
        private readonly IQueryHandler<TQuery, TResponse> _inner;
        private readonly IEnumerable<IValidator<TQuery>> _validators;

        public QueryHandler(
            IQueryHandler<TQuery, TResponse> inner,
            IEnumerable<IValidator<TQuery>> validators)
        {
            _inner = inner;
            _validators = validators;
        }

        public async Task<Result<TResponse>> Handle(TQuery query, CancellationToken cancellationToken = default)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TQuery>(query);
                var results = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
                var failures = results.SelectMany(r => r.Errors).Where(f => f is not null).ToList();

                if (failures.Count > 0)
                {
                    var message = string.Join("; ", failures.Select(f => f.ErrorMessage));
                    return Result.Failure<TResponse>(new Error("validation_failed", message, ErrorType.Validation));
                }
            }

            return await _inner.Handle(query, cancellationToken);
        }
    }
}
