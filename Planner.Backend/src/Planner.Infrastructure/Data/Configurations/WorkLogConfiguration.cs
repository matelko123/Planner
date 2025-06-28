using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Planner.Domain.Entities;

namespace Planner.Infrastructure.Data.Configurations;

public class WorkLogConfiguration : IEntityTypeConfiguration<WorkLog>
{
    public void Configure(EntityTypeBuilder<WorkLog> builder)
    {
        builder.ToTable("WorkLogs");

        builder.HasKey(wl => new { wl.UserId, wl.ProjectId, wl.Date });

        builder.Property(wl => wl.Hours)
            .IsRequired();

        builder.Property(wl => wl.Description)
            .HasMaxLength(300);

        builder.HasOne(wl => wl.User)
            .WithMany()
            .HasForeignKey(wl => wl.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(wl => wl.Project)
            .WithMany()
            .HasForeignKey(wl => wl.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
