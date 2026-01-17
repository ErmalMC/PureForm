using Microsoft.EntityFrameworkCore;
using PureForm.Application.DTOs;
using PureForm.Application.Interfaces;
using PureForm.Domain.Entities;
using PureForm.Infrastructure.Data;
using PureForm.Infrastructure.Repositories;

namespace PureForm.Infrastructure.Services;

public class FoodItemService : IFoodItemService
{
    private readonly IRepository<FoodItem> _foodRepository;
    private readonly ApplicationDbContext _context;

    public FoodItemService(IRepository<FoodItem> foodRepository, ApplicationDbContext context)
    {
        _foodRepository = foodRepository;
        _context = context;
    }

    public async Task<IEnumerable<FoodItemDto>> GetAllAsync()
    {
        var foods = await _foodRepository.GetAllAsync();
        return foods.Select(MapToDto);
    }

    public async Task<IEnumerable<FoodItemDto>> GetPopularAsync()
    {
        var foods = await _context.FoodItems
            .Where(f => f.IsPopular)
            .OrderBy(f => f.Name)
            .ToListAsync();
        return foods.Select(MapToDto);
    }

    public async Task<IEnumerable<FoodItemDto>> SearchAsync(string query)
    {
        var foods = await _context.FoodItems
            .Where(f => f.Name.ToLower().Contains(query.ToLower()))
            .Take(20)
            .ToListAsync();
        return foods.Select(MapToDto);
    }

    public async Task<IEnumerable<FoodItemDto>> GetByCategoryAsync(string category)
    {
        var foods = await _context.FoodItems
            .Where(f => f.Category == category)
            .OrderBy(f => f.Name)
            .ToListAsync();
        return foods.Select(MapToDto);
    }

    public async Task SeedPopularFoodsAsync()
    {
        // Check if already seeded
        var existingCount = await _context.FoodItems.CountAsync();
        if (existingCount > 0) return;

        var popularFoods = new List<FoodItem>
        {
            // Proteins
            new() { Name = "Chicken Breast", Category = "Protein", CaloriesPer100g = 165, ProteinPer100g = 31, CarbsPer100g = 0, FatsPer100g = 3.6m, IsPopular = true, CreatedAt = DateTime.UtcNow },
            new() { Name = "Salmon", Category = "Protein", CaloriesPer100g = 208, ProteinPer100g = 20, CarbsPer100g = 0, FatsPer100g = 13, IsPopular = true, CreatedAt = DateTime.UtcNow },
            new() { Name = "Ground Beef (90% lean)", Category = "Protein", CaloriesPer100g = 176, ProteinPer100g = 20, CarbsPer100g = 0, FatsPer100g = 10, IsPopular = true, CreatedAt = DateTime.UtcNow },
            new() { Name = "Eggs (whole)", Category = "Protein", CaloriesPer100g = 143, ProteinPer100g = 13, CarbsPer100g = 1, FatsPer100g = 10, IsPopular = true, CreatedAt = DateTime.UtcNow },
            new() { Name = "Greek Yogurt", Category = "Protein", CaloriesPer100g = 97, ProteinPer100g = 10, CarbsPer100g = 3.6m, FatsPer100g = 5, IsPopular = true, CreatedAt = DateTime.UtcNow },
            new() { Name = "Tuna (canned)", Category = "Protein", CaloriesPer100g = 116, ProteinPer100g = 26, CarbsPer100g = 0, FatsPer100g = 1, IsPopular = true, CreatedAt = DateTime.UtcNow },
            new() { Name = "Tofu", Category = "Protein", CaloriesPer100g = 76, ProteinPer100g = 8, CarbsPer100g = 1.9m, FatsPer100g = 4.8m, IsPopular = true, CreatedAt = DateTime.UtcNow },

            // Carbs
            new() { Name = "Brown Rice", Category = "Carbs", CaloriesPer100g = 111, ProteinPer100g = 2.6m, CarbsPer100g = 23, FatsPer100g = 0.9m, IsPopular = true, CreatedAt = DateTime.UtcNow },
            new() { Name = "White Rice", Category = "Carbs", CaloriesPer100g = 130, ProteinPer100g = 2.7m, CarbsPer100g = 28, FatsPer100g = 0.3m, IsPopular = true, CreatedAt = DateTime.UtcNow },
            new() { Name = "Oatmeal", Category = "Carbs", CaloriesPer100g = 389, ProteinPer100g = 17, CarbsPer100g = 66, FatsPer100g = 7, IsPopular = true, CreatedAt = DateTime.UtcNow },
            new() { Name = "Sweet Potato", Category = "Carbs", CaloriesPer100g = 86, ProteinPer100g = 1.6m, CarbsPer100g = 20, FatsPer100g = 0.1m, IsPopular = true, CreatedAt = DateTime.UtcNow },
            new() { Name = "Whole Wheat Bread", Category = "Carbs", CaloriesPer100g = 247, ProteinPer100g = 13, CarbsPer100g = 41, FatsPer100g = 3.4m, IsPopular = true, CreatedAt = DateTime.UtcNow },
            new() { Name = "Quinoa", Category = "Carbs", CaloriesPer100g = 120, ProteinPer100g = 4.4m, CarbsPer100g = 21, FatsPer100g = 1.9m, IsPopular = true, CreatedAt = DateTime.UtcNow },
            new() { Name = "Pasta", Category = "Carbs", CaloriesPer100g = 131, ProteinPer100g = 5, CarbsPer100g = 25, FatsPer100g = 1.1m, IsPopular = true, CreatedAt = DateTime.UtcNow },

            // Vegetables
            new() { Name = "Broccoli", Category = "Vegetables", CaloriesPer100g = 34, ProteinPer100g = 2.8m, CarbsPer100g = 7, FatsPer100g = 0.4m, IsPopular = true, CreatedAt = DateTime.UtcNow },
            new() { Name = "Spinach", Category = "Vegetables", CaloriesPer100g = 23, ProteinPer100g = 2.9m, CarbsPer100g = 3.6m, FatsPer100g = 0.4m, IsPopular = true, CreatedAt = DateTime.UtcNow },
            new() { Name = "Carrots", Category = "Vegetables", CaloriesPer100g = 41, ProteinPer100g = 0.9m, CarbsPer100g = 10, FatsPer100g = 0.2m, IsPopular = true, CreatedAt = DateTime.UtcNow },
            new() { Name = "Bell Peppers", Category = "Vegetables", CaloriesPer100g = 31, ProteinPer100g = 1, CarbsPer100g = 6, FatsPer100g = 0.3m, IsPopular = true, CreatedAt = DateTime.UtcNow },

            // Fruits
            new() { Name = "Banana", Category = "Fruits", CaloriesPer100g = 89, ProteinPer100g = 1.1m, CarbsPer100g = 23, FatsPer100g = 0.3m, IsPopular = true, CreatedAt = DateTime.UtcNow },
            new() { Name = "Apple", Category = "Fruits", CaloriesPer100g = 52, ProteinPer100g = 0.3m, CarbsPer100g = 14, FatsPer100g = 0.2m, IsPopular = true, CreatedAt = DateTime.UtcNow },
            new() { Name = "Blueberries", Category = "Fruits", CaloriesPer100g = 57, ProteinPer100g = 0.7m, CarbsPer100g = 14, FatsPer100g = 0.3m, IsPopular = true, CreatedAt = DateTime.UtcNow },
            new() { Name = "Strawberries", Category = "Fruits", CaloriesPer100g = 32, ProteinPer100g = 0.7m, CarbsPer100g = 7.7m, FatsPer100g = 0.3m, IsPopular = true, CreatedAt = DateTime.UtcNow },

            // Healthy Fats
            new() { Name = "Avocado", Category = "Fats", CaloriesPer100g = 160, ProteinPer100g = 2, CarbsPer100g = 9, FatsPer100g = 15, IsPopular = true, CreatedAt = DateTime.UtcNow },
            new() { Name = "Almonds", Category = "Fats", CaloriesPer100g = 579, ProteinPer100g = 21, CarbsPer100g = 22, FatsPer100g = 50, IsPopular = true, CreatedAt = DateTime.UtcNow },
            new() { Name = "Peanut Butter", Category = "Fats", CaloriesPer100g = 588, ProteinPer100g = 25, CarbsPer100g = 20, FatsPer100g = 50, IsPopular = true, CreatedAt = DateTime.UtcNow },
            new() { Name = "Olive Oil", Category = "Fats", CaloriesPer100g = 884, ProteinPer100g = 0, CarbsPer100g = 0, FatsPer100g = 100, IsPopular = true, CreatedAt = DateTime.UtcNow },

            // Dairy
            new() { Name = "Milk (whole)", Category = "Dairy", CaloriesPer100g = 61, ProteinPer100g = 3.2m, CarbsPer100g = 4.8m, FatsPer100g = 3.3m, IsPopular = true, CreatedAt = DateTime.UtcNow },
            new() { Name = "Cheddar Cheese", Category = "Dairy", CaloriesPer100g = 403, ProteinPer100g = 25, CarbsPer100g = 1.3m, FatsPer100g = 33, IsPopular = true, CreatedAt = DateTime.UtcNow },
            new() { Name = "Cottage Cheese", Category = "Dairy", CaloriesPer100g = 98, ProteinPer100g = 11, CarbsPer100g = 3.4m, FatsPer100g = 4.3m, IsPopular = true, CreatedAt = DateTime.UtcNow },
        };

        foreach (var food in popularFoods)
        {
            await _foodRepository.AddAsync(food);
        }
    }

    private static FoodItemDto MapToDto(FoodItem food) => new()
    {
        Id = food.Id,
        Name = food.Name,
        Category = food.Category,
        CaloriesPer100g = food.CaloriesPer100g,
        ProteinPer100g = food.ProteinPer100g,
        CarbsPer100g = food.CarbsPer100g,
        FatsPer100g = food.FatsPer100g,
        DefaultUnit = food.DefaultUnit,
        IsPopular = food.IsPopular
    };
}