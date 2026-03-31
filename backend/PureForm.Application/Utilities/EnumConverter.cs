using PureForm.Domain.Enums;

namespace PureForm.Application.Utilities;

/// <summary>
/// Utility class for converting between enum and string representations.
/// </summary>
public static class EnumConverter
{
    /// <summary>
    /// Safely parse a string to Gender enum, returning null if invalid.
    /// </summary>
    public static Gender? ParseGender(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        if (Enum.TryParse<Gender>(value, ignoreCase: true, out var result))
            return result;

        return null;
    }

    /// <summary>
    /// Safely parse a string to FitnessGoal enum, returning null if invalid.
    /// </summary>
    public static FitnessGoal? ParseFitnessGoal(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        if (Enum.TryParse<FitnessGoal>(value, ignoreCase: true, out var result))
            return result;

        return null;
    }

    /// <summary>
    /// Safely parse a string to MealType enum, returning null if invalid.
    /// </summary>
    public static MealType? ParseMealType(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        if (Enum.TryParse<MealType>(value, ignoreCase: true, out var result))
            return result;

        return null;
    }

    /// <summary>
    /// Safely parse a string to DifficultyLevel enum, returning null if invalid.
    /// </summary>
    public static DifficultyLevel? ParseDifficultyLevel(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        if (Enum.TryParse<DifficultyLevel>(value, ignoreCase: true, out var result))
            return result;

        return null;
    }

    /// <summary>
    /// Safely parse a string to SubscriptionStatus enum, returning null if invalid.
    /// </summary>
    public static SubscriptionStatus? ParseSubscriptionStatus(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        if (Enum.TryParse<SubscriptionStatus>(value, ignoreCase: true, out var result))
            return result;

        return null;
    }

    /// <summary>
    /// Safely parse a string to FoodCategory enum, returning null if invalid.
    /// </summary>
    public static FoodCategory? ParseFoodCategory(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        if (Enum.TryParse<FoodCategory>(value, ignoreCase: true, out var result))
            return result;

        return null;
    }

    /// <summary>
    /// Convert enum value to string representation (with spaces for display).
    /// </summary>
    public static string ToString(Gender value) => value.ToString();
    public static string ToString(FitnessGoal value) => value.ToString();
    public static string ToString(MealType value) => value.ToString();
    public static string ToString(DifficultyLevel value) => value.ToString();
    public static string ToString(SubscriptionStatus value) => value.ToString();
    public static string ToString(FoodCategory value) => value.ToString();
}

