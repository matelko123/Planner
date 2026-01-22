using Planner.Api.Extensions;

namespace Planner.Api.Features.Projects;

internal sealed class GetProjects : IEndpoint<ProjectsGroup>
{
    internal sealed record Response(int Id, string Name);

    public static void Map(RouteGroupBuilder group)
    {
        group.MapGet("/", () => Results.Ok(new[]
            {
                new Response(1, "Project Alpha"),
                new Response(2, "Project Beta"),
            }))
            .WithName("GetProjects")
            .WithSummary("Retrieves a list of projects.")
            .WithDescription("Returns a list of all projects available in the system.")
            .Produces<IEnumerable<Response>>(StatusCodes.Status200OK);
    }
}