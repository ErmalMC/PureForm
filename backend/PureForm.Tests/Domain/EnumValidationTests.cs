using PureForm.Domain.Entities;
using PureForm.Domain.Enums;
using Xunit;

namespace PureForm.Tests.Domain;

public class EnumValidationTests
{
    [Fact]
    public void GenderEnum_ShouldHaveFourValues()
    {
        // Assert
        Assert.Equal(4, Enum.GetValues(typeof(Gender)).Length);
    }

    [Fact]
    public void FitnessGoalEnum_ShouldHaveSixValues()
    {
        // Assert
        Assert.Equal(6, Enum.GetValues(typeof(FitnessGoal)).Length);
    }

    [Fact]
    public void MealTypeEnum_ShouldHaveSixValues()
    {
        // Assert
        Assert.Equal(6, Enum.GetValues(typeof(MealType)).Length);
    }

    [Fact]
    public void DifficultyLevelEnum_ShouldHaveFourValues()
    {
        // Assert
        Assert.Equal(4, Enum.GetValues(typeof(DifficultyLevel)).Length);
    }

    [Fact]
    public void SubscriptionStatusEnum_ShouldHaveSixValues()
    {
        // Assert
        Assert.Equal(6, Enum.GetValues(typeof(SubscriptionStatus)).Length);
    }

    [Fact]
    public void FoodCategoryEnum_ShouldHaveTenValues()
    {
        // Assert
        Assert.Equal(10, Enum.GetValues(typeof(FoodCategory)).Length);
    }
}

