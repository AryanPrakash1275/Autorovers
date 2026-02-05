namespace AutoroversApi.Contracts;

public sealed record ApiErrors(
    string Code,
    string Message,
    Dictionary<string, string[]>? Errors = null,
    string? TraceId = null)
{
    public static ApiErrors From(
        string code,
        string message,
        Dictionary<string, string[]>? errors = null,
        string? traceId = null)
        => new(code, message, errors, traceId);
}
