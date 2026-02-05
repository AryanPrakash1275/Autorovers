using Autorovers.Application.Abstractions.Exceptions;
using AutoroversApi.Contracts;
using Microsoft.AspNetCore.Diagnostics;

namespace AutoroversApi.Infrastructure;

internal sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(exception, "Unhandled exception occurred");

        if (exception is ValidationException vex)
        {
            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

            await httpContext.Response.WriteAsJsonAsync(
                ApiErrors.From(
                    code: "validation_failed",
                    message: vex.Message,
                    errors: vex.Errors is null
                        ? null
                        : new Dictionary<string, string[]>(vex.Errors),
                    traceId: httpContext.TraceIdentifier
                ),
                cancellationToken
            );

            return true;
        }

        if (exception is NotFoundException nf)
        {
            httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
            await httpContext.Response.WriteAsJsonAsync(
                ApiErrors.From(
                    code: "not_found",
                    message: nf.Message,
                    errors: null,
                    traceId: httpContext.TraceIdentifier
                ),
                cancellationToken
            );
            return true;
        }

        if (exception is ForbiddenAccessException fb)
        {
            httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
            await httpContext.Response.WriteAsJsonAsync(
                ApiErrors.From(
                    code: "forbidden",
                    message: fb.Message,
                    errors: null,
                    traceId: httpContext.TraceIdentifier
                ),
                cancellationToken
            );
            return true;
        }

        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await httpContext.Response.WriteAsJsonAsync(
            ApiErrors.From(
                code: "server_error",
                message: "Server failure.",
                errors: null,
                traceId: httpContext.TraceIdentifier
            ),
            cancellationToken
        );

        return true;
    }
}
