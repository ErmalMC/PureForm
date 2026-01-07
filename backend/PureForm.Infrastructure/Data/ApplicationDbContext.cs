using Microsoft.EntityFrameworkCore;
using PureForm.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PureForm.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<WorkoutPlan> WorkoutPlans { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<NutritionLog> NutritionLogs { get; set; }
        public DbSet<StripeSubscription> StripeSubscriptions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Weight).HasPrecision(5, 2);
                entity.Property(e => e.Height).HasPrecision(5, 2);
            });

            // WorkoutPlan configuration
            modelBuilder.Entity<WorkoutPlan>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.HasOne(e => e.User)
                    .WithMany(u => u.WorkoutPlans)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Exercise configuration
            modelBuilder.Entity<Exercise>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.HasOne(e => e.WorkoutPlan)
                    .WithMany(wp => wp.Exercises)
                    .HasForeignKey(e => e.WorkoutPlanId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // NutritionLog configuration
            modelBuilder.Entity<NutritionLog>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FoodName).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Calories).HasPrecision(7, 2);
                entity.Property(e => e.Protein).HasPrecision(6, 2);
                entity.Property(e => e.Carbs).HasPrecision(6, 2);
                entity.Property(e => e.Fats).HasPrecision(6, 2);
                entity.Property(e => e.ServingSize).HasPrecision(6, 2);
                entity.HasOne(e => e.User)
                    .WithMany(u => u.NutritionLogs)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // StripeSubscription configuration
            modelBuilder.Entity<StripeSubscription>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.StripeCustomerId);
                entity.HasIndex(e => e.StripeSubscriptionId);
                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }

}
