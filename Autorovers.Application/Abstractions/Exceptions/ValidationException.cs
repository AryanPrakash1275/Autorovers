namespace Autorovers.Application.Abstractions.Exceptions;

public sealed class ValidationException : Exception
{
    public ValidationException(string message) : base(message) { }
    public IReadOnlyDictionary<string, string[]> Errors { get; init; }
        = new Dictionary<string, string[]>();
}
