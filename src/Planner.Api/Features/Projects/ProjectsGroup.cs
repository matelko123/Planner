using Planner.Api.Extensions;

namespace Planner.Api.Features.Projects;

internal sealed class ProjectsGroup : IGroupEndpoint
{
    public RouteGroupBuilder InitializeGroup(IEndpointRouteBuilder app)
    {
        return app.MapGroup("/projects")
            .WithTags("Projects");
    }
}