using PureForm.Domain.Entities;
using PureForm.Domain.Enums;
using Xunit;

namespace PureForm.Tests.Domain;

public class WorkoutPlanTests
{
    [Fact]
    public void WorkoutPlan_ShouldInitializeExerciseCollection()
    {
        // Arrange & Act
        var plan = new WorkoutPlan();

        // Assert
        Assert.NotNull(plan.Exercises);
        Assert.Empty(plan.Exercises);
    }

    [Fact]
    public void WorkoutPlan_ShouldAcceptDifficultyLevelEnum()
    {
        // Arrange & Act
        var plan = new WorkoutPlan
        {
            DifficultyLevel = DifficultyLevel.Intermediate,
            Name = "Intermediate Workout",
            DurationWeeks = 8
        };

        // Assert
        Assert.Equal(DifficultyLevel.Intermediate, plan.DifficultyLevel);
    }

    [Theory]
    [InlineData(DifficultyLevel.Beginner)]
    [InlineData(DifficultyLevel.Intermediate)]
    [InlineData(DifficultyLevel.Advanced)]
    [InlineData(DifficultyLevel.Expert)]
    public void WorkoutPlan_ShouldAcceptAllDifficultyLevels(DifficultyLevel level)
    {
        // Arrange & Act
        var plan = new WorkoutPlan { DifficultyLevel = level };

        // Assert
        Assert.Equal(level, plan.DifficultyLevel);
    }
}

