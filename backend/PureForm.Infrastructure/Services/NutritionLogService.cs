// ============================================================
// PureForm.Infrastructure/Services/NutritionLogService.cs
// ============================================================
using Microsoft.EntityFrameworkCore;
using PureForm.Application.DTOs;
using PureForm.Application.Interfaces;
using PureForm.Domain.Entities;
using PureForm.Infrastructure.Data;
using PureForm.Infrastructure.Repositories;

namespace PureForm.Infrastructure.Services;

public class NutritionLogService : INutritionLogService
{
    private readonly IRepository<NutritionLog> _nutritionLogRepository;
    private readonly ApplicationDbContext _context;

    public NutritionLogService(
        IRepository<NutritionLog> nutritionLogRepository,
        ApplicationDbContext context)
    {
        _nutritionLogRepository = nutritionLogRepository;
        _context = context;
    }

    public async Task<NutritionLogDto?> GetByIdAsync(int id)
    {
        var log = await _nutritionLogRepository.GetByIdAsync(id);
        return log == null ? null : MapToDto(log);
    }

    public async Task<IEnumerable<NutritionLogDto>> GetByUserIdAsync(int userId, DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = _context.NutritionLogs
            .Where(n => n.UserId == userId);

        if (startDate.HasValue)
        {
            query = query.Where(n => n.LogDate >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(n => n.LogDate <= endDate.Value);
        }

        var logs = await query.OrderByDescending(n => n.LogDate).ToListAsync();
        return logs.Select(MapToDto);
    }

    public async Task<NutritionLogDto> CreateAsync(int userId, CreateNutritionLogDto dto)
    {
        var log = new NutritionLog
        {
            UserId = userId,
            LogDate = dto.LogDate,
            MealType = dto.MealType,
            FoodName = dto.FoodName,
            Calories = dto.Calories,
            Protein = dto.Protein,
            Carbs = dto.Carbs,
            Fats = dto.Fats,
            ServingSize = dto.ServingSize,
            ServingUnit = dto.ServingUnit,
            CreatedAt = DateTime.UtcNow
        };

        var created = await _nutritionLogRepository.AddAsync(log);
        return MapToDto(created);
    }

    public async Task<NutritionLogDto?> UpdateAsync(int id, CreateNutritionLogDto dto)
    {
        var log = await _nutritionLogRepository.GetByIdAsync(id);
        if (log == null) return null;

        log.LogDate = dto.LogDate;
        log.MealType = dto.MealType;
        log.FoodName = dto.FoodName;
        log.Calories = dto.Calories;
        log.Protein = dto.Protein;
        log.Carbs = dto.Carbs;
        log.Fats = dto.Fats;
        log.ServingSize = dto.ServingSize;
        log.ServingUnit = dto.ServingUnit;

        await _nutritionLogRepository.UpdateAsync(log);
        return MapToDto(log);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var log = await _nutritionLogRepository.GetByIdAsync(id);
        if (log == null) return false;

        await _nutritionLogRepository.DeleteAsync(log);
        return true;
    }

    public async Task<Dictionary<string, decimal>> GetDailyTotalsAsync(int userId, DateTime date)
    {
        var startOfDay = date.Date;
        var endOfDay = startOfDay.AddDays(1);

        var logs = await _context.NutritionLogs
            .Where(n => n.UserId == userId && n.LogDate >= startOfDay && n.LogDate < endOfDay)
            .ToListAsync();

        return new Dictionary<string, decimal>
        {
            { "Calories", logs.Sum(l => l.Calories) },
            { "Protein", logs.Sum(l => l.Protein) },
            { "Carbs", logs.Sum(l => l.Carbs) },
            { "Fats", logs.Sum(l => l.Fats) }
        };
    }

    private static NutritionLogDto MapToDto(NutritionLog log) => new()
    {
        Id = log.Id,
        UserId = log.UserId,
        LogDate = log.LogDate,
        MealType = log.MealType,
        FoodName = log.FoodName,
        Calories = log.Calories,
        Protein = log.Protein,
        Carbs = log.Carbs,
        Fats = log.Fats,
        ServingSize = log.ServingSize,
        ServingUnit = log.ServingUnit
    };
}