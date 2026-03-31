using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PureForm.Domain.Entities;

namespace PureForm.Infrastructure.Data.Configurations;

public class NutritionLogConfiguration : IEntityTypeConfiguration<NutritionLog>
{
    public void Configure(EntityTypeBuilder<NutritionLog> entity)
    {
        entity.HasKey(e => e.Id);
        entity.Property(e => e.FoodName).IsRequired().HasMaxLength(200);
        entity.Property(e => e.MealType).IsRequired();
        entity.Property(e => e.Calories).HasPrecision(7, 2);
        entity.Property(e => e.Protein).HasPrecision(6, 2);
        entity.Property(e => e.Carbs).HasPrecision(6, 2);
        entity.Property(e => e.Fats).HasPrecision(6, 2);
        entity.Property(e => e.ServingSize).HasPrecision(6, 2);
        entity.Property(e => e.ServingUnit).IsRequired().HasMaxLength(20);
        entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        entity.Property(e => e.UpdatedAt).ValueGeneratedOnUpdate();
        entity.HasIndex(e => new { e.UserId, e.LogDate });
        entity.HasOne(e => e.User)
            .WithMany(u => u.NutritionLogs)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

