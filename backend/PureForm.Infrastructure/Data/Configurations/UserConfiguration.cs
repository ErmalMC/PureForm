using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PureForm.Domain.Entities;

namespace PureForm.Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> entity)
    {
        entity.HasKey(e => e.Id);
        entity.HasIndex(e => e.Email).IsUnique();
        entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
        entity.Property(e => e.PasswordHash).IsRequired().HasMaxLength(500);
        entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
        entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
        entity.Property(e => e.Gender).IsRequired();
        entity.Property(e => e.FitnessGoal).IsRequired();
        entity.Property(e => e.Weight).HasPrecision(5, 2);
        entity.Property(e => e.Height).HasPrecision(5, 2);
        entity.Property(e => e.DailyCalorieGoal).HasPrecision(7, 2);
        entity.Property(e => e.DailyProteinGoal).HasPrecision(6, 2);
        entity.Property(e => e.DailyCarbsGoal).HasPrecision(6, 2);
        entity.Property(e => e.DailyFatsGoal).HasPrecision(6, 2);
        entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        entity.Property(e => e.UpdatedAt).ValueGeneratedOnUpdate();
        entity.HasIndex(e => e.CreatedAt);
    }
}

