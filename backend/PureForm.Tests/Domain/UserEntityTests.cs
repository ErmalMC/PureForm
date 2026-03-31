using PureForm.Domain.Entities;
using PureForm.Domain.Enums;
using Xunit;

namespace PureForm.Tests.Domain;

public class UserEntityTests
{
    [Fact]
    public void User_ShouldInitializeWithDefaultValues()
    {
        // Arrange
        var user = new User();

        // Act & Assert
        Assert.NotNull(user.WorkoutPlans);
        Assert.NotNull(user.NutritionLogs);
        Assert.Empty(user.WorkoutPlans);
        Assert.Empty(user.NutritionLogs);
    }

    [Fact]
    public void User_ShouldAcceptGenderEnum()
    {
        // Arrange & Act
        var user = new User
        {
            Id = 1,
            Email = "test@example.com",
            FirstName = "John",
            LastName = "Doe",
            Gender = Gender.Male,
            FitnessGoal = FitnessGoal.WeightLoss
        };

        // Assert
        Assert.Equal(Gender.Male, user.Gender);
        Assert.Equal(FitnessGoal.WeightLoss, user.FitnessGoal);
    }

    [Theory]
    [InlineData(Gender.Male)]
    [InlineData(Gender.Female)]
    [InlineData(Gender.Other)]
    [InlineData(Gender.PreferNotToSay)]
    public void User_ShouldAcceptAllGenderValues(Gender gender)
    {
        // Arrange & Act
        var user = new User { Gender = gender };

        // Assert
        Assert.Equal(gender, user.Gender);
    }
}

