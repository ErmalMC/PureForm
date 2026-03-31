using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PureForm.Domain.Entities;

namespace PureForm.Infrastructure.Data.Configurations;

public class ExerciseConfiguration : IEntityTypeConfiguration<Exercise>
{
    public void Configure(EntityTypeBuilder<Exercise> entity)
    {
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
        entity.Property(e => e.Description).HasMaxLength(1000);
        entity.Property(e => e.MuscleGroup).HasMaxLength(100);
        entity.Property(e => e.Equipment).HasMaxLength(100);
        entity.Property(e => e.VideoUrl).HasMaxLength(2048);
        entity.Property(e => e.ImageUrl).HasMaxLength(2048);
        entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        entity.Property(e => e.UpdatedAt).ValueGeneratedOnUpdate();
        entity.HasIndex(e => e.WorkoutPlanId);
        entity.HasIndex(e => new { e.WorkoutPlanId, e.OrderIndex });
        entity.HasOne(e => e.WorkoutPlan)
            .WithMany(wp => wp.Exercises)
            .HasForeignKey(e => e.WorkoutPlanId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

