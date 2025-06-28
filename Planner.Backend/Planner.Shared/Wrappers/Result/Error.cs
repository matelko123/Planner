using System.Net;

namespace Planner.Shared.Wrappers.Result;

public record Error(HttpStatusCode Code, string Description);