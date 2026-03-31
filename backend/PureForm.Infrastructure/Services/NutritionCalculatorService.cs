using System.Globalization;
using PureForm.Application.DTOs;
using PureForm.Application.Interfaces;
using PureForm.Application.Utilities;
using PureForm.Domain.Entities;
using PureForm.Domain.Enums;
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
        if (user.Gender.ToString().ToLower(CultureInfo.InvariantCulture) == "male")
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
        switch (user.FitnessGoal)
        {
            case FitnessGoal.WeightLoss:
                recommendedCalories = tdee - 500;
                proteinMultiplier = 2.2m;
                explanation = "500 calorie deficit for healthy weight loss (~0.5kg/week). High protein to preserve muscle mass.";
                break;
            case FitnessGoal.MuscleGain:
                recommendedCalories = tdee + 400;
                proteinMultiplier = 2.0m;
                explanation = "400 calorie surplus for lean muscle gain. High protein intake to support muscle growth.";
                break;
            case FitnessGoal.Endurance:
                recommendedCalories = tdee;
                proteinMultiplier = 1.6m;
                explanation = "Maintenance calories with emphasis on carbohydrates for sustained energy during endurance training.";
                break;
            case FitnessGoal.GeneralFitness:
            default:
                recommendedCalories = tdee;
                proteinMultiplier = 1.8m;
                explanation = "Maintenance calories to support general fitness and health goals.";
                break;
        }
        decimal proteinGrams = user.Weight * proteinMultiplier;
        decimal carbCalories = recommendedCalories * 0.4m;
        decimal fatCalories = recommendedCalories * 0.25m;
        decimal carbGrams = carbCalories / 4;
        decimal fatGrams = fatCalories / 9;
        return new CalorieRecommendationDto
        {
            BMR = bmr,
            TDEE = tdee,
            RecommendedCalories = recommendedCalories,
            RecommendedProtein = Math.Round(proteinGrams, 1),
            RecommendedCarbs = Math.Round(carbGrams, 1),
            RecommendedFats = Math.Round(fatGrams, 1),
            Explanation = explanation
        };
    }
}
