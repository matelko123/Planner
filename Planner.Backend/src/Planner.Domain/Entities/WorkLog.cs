using System.ComponentModel.DataAnnotations;
using System.Net;
using Planner.Shared.Wrappers.Result;

namespace Planner.Domain.Entities;

public sealed class WorkLog
{
    public WorkLog(Guid userId, Guid projectId, DateOnly date, decimal hours, string? description)
    {
        UserId = userId;
        ProjectId = projectId;
        Date = date;
        Hours = hours;
        Description = description;
    }

    public Guid UserId { get; set; }
    public User User { get; set; } = default!;

    public Guid ProjectId { get; set; }
    public Project Project { get; set; } = default!;

    public DateOnly Date { get; set; }

    [Range(0, 24)]
    public decimal Hours { get; set; }

    [MaxLength(300)]
    public string? Description { get; set; } = default!;

    public static class Errors
    {
        public static readonly Error WorkLogAlreadyExists = new(HttpStatusCode.Conflict, "Work log for this user and project on this date already exists.");
        public static readonly Error WorkLogTooManyHours = new(HttpStatusCode.BadRequest, "Total work hours for the day cannot exceed 24 hours.");
    }
}