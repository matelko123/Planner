using FastEndpoints;

namespace Planner.Api.Endpoints.UserProjects;

internal sealed class UserProjectsGroup : Group
{
    public UserProjectsGroup()
    {
        Configure("user-projects", _ => {});
    }
}