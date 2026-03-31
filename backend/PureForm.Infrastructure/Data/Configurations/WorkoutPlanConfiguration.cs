using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PureForm.Domain.Entities;

namespace PureForm.Infrastructure.Data.Configurations;

public class WorkoutPlanConfiguration : IEntityTypeConfiguration<WorkoutPlan>
{
    public void Configure(EntityTypeBuilder<WorkoutPlan> entity)
    {
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
        entity.Property(e => e.Description).HasMaxLength(1000);
        entity.Property(e => e.DifficultyLevel).IsRequired();
        entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        entity.Property(e => e.UpdatedAt).ValueGeneratedOnUpdate();
        entity.HasIndex(e => e.UserId);
        entity.HasIndex(e => new { e.UserId, e.CreatedAt });
        entity.HasOne(e => e.User)
            .WithMany(u => u.WorkoutPlans)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

