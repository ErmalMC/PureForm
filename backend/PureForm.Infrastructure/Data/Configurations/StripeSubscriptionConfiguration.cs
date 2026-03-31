using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PureForm.Domain.Entities;

namespace PureForm.Infrastructure.Data.Configurations;

public class FoodItemConfiguration : IEntityTypeConfiguration<FoodItem>
{
    public void Configure(EntityTypeBuilder<FoodItem> entity)
    {
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
        entity.Property(e => e.Category).IsRequired();
        entity.Property(e => e.DefaultUnit).IsRequired().HasMaxLength(20);
        entity.Property(e => e.CaloriesPer100g).HasPrecision(7, 2);
        entity.Property(e => e.ProteinPer100g).HasPrecision(6, 2);
        entity.Property(e => e.CarbsPer100g).HasPrecision(6, 2);
        entity.Property(e => e.FatsPer100g).HasPrecision(6, 2);
        entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        entity.Property(e => e.UpdatedAt).ValueGeneratedOnUpdate();
        entity.HasIndex(e => e.Name);
        entity.HasIndex(e => e.Category);
        entity.HasIndex(e => e.CreatedAt);
    }
}

