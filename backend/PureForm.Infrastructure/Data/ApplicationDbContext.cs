using Microsoft.EntityFrameworkCore;
using PureForm.Domain.Entities;
using PureForm.Infrastructure.Data.Configurations;

namespace PureForm.Infrastructure.Data;

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
    public DbSet<FoodItem> FoodItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply configurations from separate configuration classes
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new WorkoutPlanConfiguration());
        modelBuilder.ApplyConfiguration(new ExerciseConfiguration());
        modelBuilder.ApplyConfiguration(new NutritionLogConfiguration());
        modelBuilder.ApplyConfiguration(new StripeSubscriptionConfiguration());
        modelBuilder.ApplyConfiguration(new FoodItemConfiguration());
    }

}
