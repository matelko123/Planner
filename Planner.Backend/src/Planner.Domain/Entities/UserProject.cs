using System.Net;
using Planner.Shared.Wrappers.Result;

namespace Planner.Domain.Entities;

public sealed class UserProject
{
    public UserProject(Guid userId, Guid projectId)
    {
        UserId = userId;
        ProjectId = projectId;
    }

    public Guid UserId { get; set; }
    public User User { get; set; } = default!;
    
    public Guid ProjectId { get; set; }
    public Project Project { get; set; } = default!;

    public static class Errors
    {
        public static readonly Error UserAlreadyInProject = new(HttpStatusCode.Conflict, "User is already in the project.");
    }
}