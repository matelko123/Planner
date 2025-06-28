using FastEndpoints;

namespace Planner.Api.Endpoints.Projects
{
    internal sealed class ProjectsGroup : Group
    {
        public ProjectsGroup()
        {
            Configure("projects", _ => {});
        }
    }
}