using Planner.Api.Extensions;

namespace Planner.Api.Features.Projects;

internal sealed class GetProjectById : IEndpoint<ProjectsGroup>
{
    internal sealed record Response(int Id, string Name, string? Description);

    public static void Map(RouteGroupBuilder group)
    {
        group.MapGet("/{id:int}", (int id) => 
            {
                // Simulate fetching a project
                if (id <= 0)
                    return Results.BadRequest("Invalid project ID");
                
                if (id > 100)
                    return Results.NotFound($"Project with ID {id} not found");
                
                var project = new Response(id, $"Project {id}", $"Description for project {id}");
                return Results.Ok(project);
            })
            .WithName(nameof(GetProjectById))
            .WithSummary("Gets a project by ID.")
            .WithDescription("Returns a single project identified by the provided ID.")
            .Produces<Response>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound);
    }
}

