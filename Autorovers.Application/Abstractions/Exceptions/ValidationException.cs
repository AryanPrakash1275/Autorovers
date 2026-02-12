namespace Autorovers.Application.Abstractions.Exceptions;

public sealed class ValidationException : Exception
{
    public ValidationException(string message) : base(message) { }

    public ValidationException(string message, IReadOnlyDictionary<string, string[]> errors) : base(message)
    {
        Errors = errors;
    }

    public IReadOnlyDictionary<string, string[]>? Errors { get; }
}
