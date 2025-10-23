using Autorovers.Application.Abstractions.Messaging;
using Autorovers.Application.Todos.Create;
using Autorovers.Domain.Todos;
using Autorovers.Common;
using AutoroversApi.Extensions;
using AutoroversApi.Infrastructure;

namespace AutoroversApi.Endpoints.Todos;

internal sealed class Create : IEndpoint
{
    public sealed class Request
    {
        public Guid UserId { get; init; }
        public required string Description { get; init; }  // <- fixes CS8618
        public DateTime? DueDate { get; init; }
        public List<string> Labels { get; init; } = new();
        public int Priority { get; init; }                 // from client as int
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/todos", async (
            Request request,
            ICommandHandler<CreateTodoCommand, Guid> handler,
            CancellationToken ct) =>
        {
            if (request.UserId == Guid.Empty)
                return Results.BadRequest(new { error = "UserId is required." });

            if (string.IsNullOrWhiteSpace(request.Description))
                return Results.BadRequest(new { error = "Description is required." });

            // Validate enum input safely
            if (!Enum.IsDefined(typeof(Priority), request.Priority))
                return Results.BadRequest(new { error = $"Priority must be one of {(int)Priority.Low}-{(int)Priority.High}." });

            var command = new CreateTodoCommand
            {
                UserId = request.UserId,
                Description = request.Description.Trim(),
                DueDate = request.DueDate,
                Labels = request.Labels ?? new List<string>(),
                Priority = (Priority)request.Priority
            };

            Result<Guid> result = await handler.Handle(command, ct);
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Todos)
        .RequireAuthorization(); // keep if auth is actually configured
    }
}
