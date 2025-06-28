using Microsoft.EntityFrameworkCore;
using Planner.Application.Data;
using Planner.Domain.Entities;

namespace Planner.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) 
    : DbContext(options), IAppDbContext
{
    public DbSet<User> Users { get; set; } = default!;
    public DbSet<Project> Projects { get; set; } = default!;
    public DbSet<UserProject> UserProjects { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("Planner");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}