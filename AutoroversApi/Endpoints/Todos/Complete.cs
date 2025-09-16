using System;
using System.Threading;
using System.Threading.Tasks;
using Autorovers.Application.Abstractions.Messaging;
using Autorovers.Application.Todos.Complete;
using Autorovers.Common;                         // Result
using AutoroversApi.Endpoints;                  // CustomResults, Tags (your API project namespace)
using Microsoft.AspNetCore.Builder;              // for MapPut, Results
using Microsoft.AspNetCore.Routing;              // for IEndpointRouteBuilder

namespace AutoroversApi.Endpoints.Todos
{
    internal sealed class Complete : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut("todos/{id:guid}/complete", async (
                Guid id,
                ICommandHandler<CompleteTodoCommand> handler,
                CancellationToken cancellationToken) =>
            {
                var command = new CompleteTodoCommand(id);

                Result result = await handler.Handle(command, cancellationToken);

                return result;
            })
            .WithTags(Tags.Todos)
            .RequireAuthorization();
        }
    }
}
