using Microsoft.EntityFrameworkCore;
using PureForm.Domain.Entities;
using PureForm.Domain.Enums;
using PureForm.Infrastructure.Data;
using Xunit;

namespace PureForm.Tests.Infrastructure;

public class DatabaseIntegrationTests
{
    private ApplicationDbContext CreateInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    [Fact]
    public async Task User_ShouldBeSavedAndRetrieved()
    {
        // Arrange
        var context = CreateInMemoryContext();
        var user = new User
        {
            Email = "test@example.com",
            PasswordHash = "hashed_password",
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = new DateTime(1990, 1, 1),
            Weight = 75.5m,
            Height = 180,
            Gender = Gender.Male,
            FitnessGoal = FitnessGoal.WeightLoss,
            IsPremium = false,
            CreatedAt = DateTime.UtcNow
        };

        // Act
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        var retrievedUser = await context.Users.FirstOrDefaultAsync(u => u.Email == "test@example.com");

        // Assert
        Assert.NotNull(retrievedUser);
        Assert.Equal("test@example.com", retrievedUser.Email);
        Assert.Equal("John", retrievedUser.FirstName);
        Assert.Equal(Gender.Male, retrievedUser.Gender);
    }

    [Fact]
    public async Task WorkoutPlan_ShouldCascadeDeleteExercises()
    {
        // Arrange
        var context = CreateInMemoryContext();
        var user = new User
        {
            Email = "test@example.com",
            PasswordHash = "hash",
            FirstName = "John",
            LastName = "Doe",
            Gender = Gender.Male,
            FitnessGoal = FitnessGoal.WeightLoss,
            CreatedAt = DateTime.UtcNow
        };

        var plan = new WorkoutPlan
        {
            Name = "Beginner Plan",
            Description = "For beginners",
            DifficultyLevel = DifficultyLevel.Beginner,
            DurationWeeks = 12,
            User = user,
            CreatedAt = DateTime.UtcNow
        };

        var exercise = new Exercise
        {
            Name = "Push-ups",
            Description = "Standard push-ups",
            MuscleGroup = "Chest",
            Equipment = "None",
            Sets = 3,
            Reps = 10,
            OrderIndex = 1,
            WorkoutPlan = plan,
            CreatedAt = DateTime.UtcNow
        };

        plan.Exercises.Add(exercise);

        // Act
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        var planId = plan.Id;
        var exerciseId = exercise.Id;

        context.WorkoutPlans.Remove(plan);
        await context.SaveChangesAsync();

        var deletedExercise = await context.Exercises.FirstOrDefaultAsync(e => e.Id == exerciseId);

        // Assert
        Assert.Null(deletedExercise);
    }

    [Fact]
    public async Task NutritionLog_ShouldHaveCorrectMealType()
    {
        // Arrange
        var context = CreateInMemoryContext();
        var user = new User
        {
            Email = "test@example.com",
            PasswordHash = "hash",
            FirstName = "Jane",
            LastName = "Doe",
            Gender = Gender.Female,
            FitnessGoal = FitnessGoal.WeightLoss,
            CreatedAt = DateTime.UtcNow
        };

        var log = new NutritionLog
        {
            FoodName = "Chicken Breast",
            MealType = MealType.Lunch,
            LogDate = DateTime.UtcNow,
            Calories = 165,
            Protein = 31,
            Carbs = 0,
            Fats = 3.6m,
            ServingSize = 100,
            ServingUnit = "g",
            User = user,
            CreatedAt = DateTime.UtcNow
        };

        // Act
        await context.Users.AddAsync(user);
        await context.NutritionLogs.AddAsync(log);
        await context.SaveChangesAsync();

        var retrievedLog = await context.NutritionLogs.FirstOrDefaultAsync(l => l.FoodName == "Chicken Breast");

        // Assert
        Assert.NotNull(retrievedLog);
        Assert.Equal(MealType.Lunch, retrievedLog.MealType);
        Assert.Equal(165, retrievedLog.Calories);
    }
}

