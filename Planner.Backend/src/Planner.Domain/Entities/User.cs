using System.ComponentModel.DataAnnotations;
using System.Net;
using Planner.Shared.Wrappers.Result;

namespace Planner.Domain.Entities;

public sealed class User
{
    public User(Guid userId, string firstName, string lastName, string email)
    {
        UserId = userId;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }

    public Guid UserId { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    [EmailAddress]
    public string Email { get; set; } = default!;
    public ICollection<Project> OwnedProjects { get; set; } = [];
    public ICollection<UserProject> UserProjects { get; set; } = [];

    public static class Errors
    {
        public static Error UserNotFound = new(HttpStatusCode.NotFound, "User not found.");
        public static Error UserAlreadyExists = new(HttpStatusCode.Conflict, "User already exists.");
    }
}