using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PureForm.Domain.Entities;

namespace PureForm.Infrastructure.Data.Configurations;

public class StripeSubscriptionConfiguration : IEntityTypeConfiguration<StripeSubscription>
{
    public void Configure(EntityTypeBuilder<StripeSubscription> entity)
    {
        entity.HasKey(e => e.Id);
        entity.Property(e => e.StripeCustomerId).IsRequired().HasMaxLength(255);
        entity.Property(e => e.StripeSubscriptionId).IsRequired().HasMaxLength(255);
        entity.Property(e => e.Status).IsRequired();
        entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        entity.Property(e => e.UpdatedAt).ValueGeneratedOnUpdate();
        entity.HasIndex(e => e.StripeCustomerId).IsUnique();
        entity.HasIndex(e => e.StripeSubscriptionId).IsUnique();
        entity.HasIndex(e => e.UserId);
        entity.HasOne(e => e.User)
            .WithMany()
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

