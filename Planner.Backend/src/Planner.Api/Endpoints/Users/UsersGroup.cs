using FastEndpoints;

namespace Planner.Api.Endpoints.Users;

internal sealed class UsersGroup : Group
{
    public UsersGroup()
    {
        Configure("users", _ => {});
    }
}