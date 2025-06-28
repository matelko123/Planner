using System.ComponentModel.DataAnnotations;
using System.Net;
using Planner.Shared.Wrappers.Result;

namespace Planner.Domain.Entities;

public sealed class Project
{
    public Project(string name, string description, Guid ownerId)
    {
        ProjectId = Guid.NewGuid();
        Name = name;
        Description = description;
        OwnerId = ownerId;
    }

    public Guid ProjectId { get; set; }
    [MaxLength(50)]
    public string Name { get; set; } = default!;
    [MaxLength(100)]
    public string Description { get; set; } = default!;
    public Guid OwnerId { get; set; }
    public User Owner { get; set; } = default!;
    
    public ICollection<UserProject> UserProjects { get; set; } = [];

    public static class Errors
    {
        public static Error ProjectNotFound = new(HttpStatusCode.NotFound, "Project not found.");
        public static Error ProjectExists = new(HttpStatusCode.Conflict, "Project already exists.");
    }
}