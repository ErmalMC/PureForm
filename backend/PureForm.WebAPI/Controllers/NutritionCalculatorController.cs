using Microsoft.AspNetCore.Mvc;
using PureForm.Application.Interfaces;

namespace PureForm.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NutritionCalculatorController : ControllerBase
{
    private readonly INutritionCalculatorService _calculatorService;
    private readonly IUserService _userService;

    public NutritionCalculatorController(
        INutritionCalculatorService calculatorService,
        IUserService userService)
    {
        _calculatorService = calculatorService;
        _userService = userService;
    }

    [HttpGet("recommendations/{userId}")]
    public async Task<IActionResult> GetRecommendations(int userId)
    {
        try
        {
            var recommendations = await _calculatorService.CalculateRecommendedIntakeAsync(userId);
            return Ok(recommendations);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost("apply-recommendations/{userId}")]
    public async Task<IActionResult> ApplyRecommendations(int userId)
    {
        try
        {
            var recommendations = await _calculatorService.CalculateRecommendedIntakeAsync(userId);

            var updateDto = new PureForm.Application.DTOs.UpdateUserDto
            {
                DailyCalorieGoal = recommendations.RecommendedCalories,
                DailyProteinGoal = recommendations.RecommendedProtein,
                DailyCarbsGoal = recommendations.RecommendedCarbs,
                DailyFatsGoal = recommendations.RecommendedFats
            };

            var updated = await _userService.UpdateAsync(userId, updateDto);
            return Ok(new { message = "Goals updated successfully", user = updated });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}