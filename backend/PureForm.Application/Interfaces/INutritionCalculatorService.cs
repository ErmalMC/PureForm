namespace PureForm.Application.Interfaces;

using PureForm.Application.DTOs;

public interface INutritionCalculatorService
{
    Task<CalorieRecommendationDto> CalculateRecommendedIntakeAsync(int userId);
}