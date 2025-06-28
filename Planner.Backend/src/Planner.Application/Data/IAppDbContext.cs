using Microsoft.EntityFrameworkCore;
using Planner.Domain.Entities;

namespace Planner.Application.Data;

public interface IAppDbContext
{
    DbSet<User> Users { get; set; }
    DbSet<Project> Projects { get; set; }
    DbSet<UserProject> UserProjects { get; set; }

    Task<int> SaveChangesAsync(CancellationToken ct = default);
}