using Planner.Api.Extensions;

namespace Planner.Api.Features.Projects;

internal sealed class CreateProject : IEndpoint<ProjectsGroup>
{
    internal sealed record Request(string Name, string? Description);

    internal sealed record Response(int Id, string Name, string? Description);

    public static void Map(RouteGroupBuilder group)
    {
        group.MapPost("/", (Request request) =>
            {
                // Simulate project creation
                var newProject = new Response(3, request.Name, request.Description);
                return Results.CreatedAtRoute(nameof(GetProjectById), new { id = newProject.Id }, newProject);
            })
            .WithName("CreateProject")
            .WithSummary("Creates a new project.")
            .WithDescription("Creates a new project with the given name and optional description.")
            .Produces<Response>(StatusCodes.Status201Created)
            .ProducesValidationProblem();
    }
}