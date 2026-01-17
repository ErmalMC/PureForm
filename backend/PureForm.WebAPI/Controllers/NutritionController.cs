// ============================================================
// PureForm.WebAPI/Controllers/NutritionController.cs
// ============================================================
using Microsoft.AspNetCore.Mvc;
using PureForm.Application.DTOs;
using PureForm.Application.Interfaces;

namespace PureForm.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NutritionController : ControllerBase
{
    private readonly INutritionLogService _nutritionLogService;

    public NutritionController(INutritionLogService nutritionLogService)
    {
        _nutritionLogService = nutritionLogService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<NutritionLogDto>> GetById(int id)
    {
        var log = await _nutritionLogService.GetByIdAsync(id);
        if (log == null) return NotFound();
        return Ok(log);
    }

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<NutritionLogDto>>> GetByUserId(
        int userId,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null)
    {
        var logs = await _nutritionLogService.GetByUserIdAsync(userId, startDate, endDate);
        return Ok(logs);
    }

    [HttpPost("user/{userId}")]
    public async Task<ActionResult<NutritionLogDto>> Create(int userId, [FromBody] CreateNutritionLogDto dto)
    {
        var log = await _nutritionLogService.CreateAsync(userId, dto);
        return CreatedAtAction(nameof(GetById), new { id = log.Id }, log);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<NutritionLogDto>> Update(int id, [FromBody] CreateNutritionLogDto dto)
    {
        var log = await _nutritionLogService.UpdateAsync(id, dto);
        if (log == null) return NotFound();
        return Ok(log);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _nutritionLogService.DeleteAsync(id);
        if (!result) return NotFound();
        return NoContent();
    }

    [HttpGet("user/{userId}/daily-totals")]
    public async Task<ActionResult<Dictionary<string, decimal>>> GetDailyTotals(
        int userId,
        [FromQuery] DateTime date)
    {
        var totals = await _nutritionLogService.GetDailyTotalsAsync(userId, date);
        return Ok(totals);
    }
}