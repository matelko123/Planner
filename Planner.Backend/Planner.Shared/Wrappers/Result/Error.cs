using System.Net;

namespace Planner.Shared.Wrappers.Result;

public record Error(HttpStatusCode Code, string Description)
{
    public static implicit operator bool(Error? error) => error is not null;
}