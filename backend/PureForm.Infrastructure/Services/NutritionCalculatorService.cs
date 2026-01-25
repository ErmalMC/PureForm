using PureForm.Application.DTOs;
using PureForm.Application.Interfaces;
using PureForm.Domain.Entities;
using PureForm.Infrastructure.Repositories;

namespace PureForm.Infrastructure.Services;

public class NutritionCalculatorService : INutritionCalculatorService
{
    private readonly IRepository<User> _userRepository;

    public NutritionCalculatorService(IRepository<User> userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<CalorieRecommendationDto> CalculateRecommendedIntakeAsync(int userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null) throw new Exception("User not found");

        var age = DateTime.UtcNow.Year - user.DateOfBirth.Year;
        if (DateTime.UtcNow.DayOfYear < user.DateOfBirth.DayOfYear) age--;

        // Calculate BMR using Mifflin-St Jeor Equation
        decimal bmr;
        if (user.Gender.ToLower() == "male")
        {
            bmr = (10 * user.Weight) + (6.25m * user.Height) - (5 * age) + 5;
        }
        else
        {
            bmr = (10 * user.Weight) + (6.25m * user.Height) - (5 * age) - 161;
        }


        decimal tdee = bmr * 1.55m;

        // Adjust based on fitness goal
        decimal recommendedCalories;
        decimal proteinMultiplier;
        string explanation;

        switch (user.FitnessGoal.ToLower())
        {
            case "weight loss":
                // 500 calorie deficit for ~1 lb/week loss
                recommendedCalories = tdee - 500;
                proteinMultiplier = 2.2m; // Higher protein to preserve muscle
                explanation = "500 calorie deficit for healthy weight loss (~0.5kg/week). High protein to preserve muscle mass.";
                break;

            case "muscle gain":
                // 300-500 calorie surplus
                recommendedCalories = tdee + 400;
                proteinMultiplier = 2.0m; // High protein for muscle building
                explanation = "400 calorie surplus for lean muscle gain. High protein intake to support muscle growth.";
                break;

            case "endurance":
                // Maintenance with higher carbs
                recommendedCalories = tdee;
                proteinMultiplier = 1.6m;
                explanation = "Maintenance calories with emphasis on carbohydrates for sustained energy during endurance training.";
                break;

            case "general fitness":
            default:
                // Maintenance
                recommendedCalories = tdee;
                proteinMultiplier = 1.8m;
                explanation = "Maintenance calories to support general fitness and health goals.";
                break;
        }


        decimal proteinGrams = user.Weight * proteinMultiplier; // grams per kg body weight
        decimal proteinCalories = proteinGrams * 4; // 4 calories per gram


        decimal fatCalories = recommendedCalories * 0.28m;
        decimal fatGrams = fatCalories / 9; // 9 calories per gram


        decimal carbCalories = recommendedCalories - proteinCalories - fatCalories;
        decimal carbGrams = carbCalories / 4; // 4 calories per gram

        return new CalorieRecommendationDto
        {
            BMR = Math.Round(bmr, 0),
            TDEE = Math.Round(tdee, 0),
            RecommendedCalories = Math.Round(recommendedCalories, 0),
            RecommendedProtein = Math.Round(proteinGrams, 0),
            RecommendedCarbs = Math.Round(carbGrams, 0),
            RecommendedFats = Math.Round(fatGrams, 0),
            Explanation = explanation
        };
    }
}