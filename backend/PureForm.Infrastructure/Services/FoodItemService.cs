using Microsoft.EntityFrameworkCore;
using PureForm.Application.DTOs;
using PureForm.Application.Interfaces;
using PureForm.Application.Utilities;
using PureForm.Domain.Entities;
using PureForm.Domain.Enums;
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
        var foods = await _context.FoodItems.OrderBy(f => f.Name).ToListAsync();
        return foods.Select(MapToDto);
    }
    public async Task<IEnumerable<FoodItemDto>> SearchAsync(string query)
    {
        var foods = await _context.FoodItems
            .Where(f => f.Name.ToLower().Contains(query.ToLower()))
            .Take(20).ToListAsync();
        return foods.Select(MapToDto);
    }
    public async Task<IEnumerable<FoodItemDto>> GetByCategoryAsync(string category)
    {
        var categoryEnum = EnumConverter.ParseFoodCategory(category);
        if (!categoryEnum.HasValue) return new List<FoodItemDto>();
        var foods = await _context.FoodItems
            .Where(f => f.Category == categoryEnum.Value)
            .OrderBy(f => f.Name).ToListAsync();
        return foods.Select(MapToDto);
    }
    public async Task SeedPopularFoodsAsync()
    {
        var existingCount = await _context.FoodItems.CountAsync();
        if (existingCount > 0)
        {
            Console.WriteLine($"Database already has {existingCount} food items. Skipping seed.");
            return;
        }
        var popularFoods = new List<FoodItem>
        {
            new() { Name = "Chicken Breast", Category = FoodCategory.Protein, CaloriesPer100g = 165, ProteinPer100g = 31, CarbsPer100g = 0, FatsPer100g = 3.6m, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },
            new() { Name = "Turkey Breast", Category = FoodCategory.Protein, CaloriesPer100g = 135, ProteinPer100g = 30, CarbsPer100g = 0, FatsPer100g = 1, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },
            new() { Name = "Salmon", Category = FoodCategory.Protein, CaloriesPer100g = 208, ProteinPer100g = 20, CarbsPer100g = 0, FatsPer100g = 13, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },
            new() { Name = "Eggs (whole)", Category = FoodCategory.Protein, CaloriesPer100g = 143, ProteinPer100g = 13, CarbsPer100g = 1, FatsPer100g = 10, DefaultUnit = "piece", CreatedAt = DateTime.UtcNow },
            new() { Name = "Tofu", Category = FoodCategory.Protein, CaloriesPer100g = 76, ProteinPer100g = 8, CarbsPer100g = 1.9m, FatsPer100g = 4.8m, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },
            new() { Name = "Brown Rice (cooked)", Category = FoodCategory.Carbohydrates, CaloriesPer100g = 111, ProteinPer100g = 2.6m, CarbsPer100g = 23, FatsPer100g = 0.9m, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },
            new() { Name = "Oatmeal (cooked)", Category = FoodCategory.Carbohydrates, CaloriesPer100g = 71, ProteinPer100g = 2.5m, CarbsPer100g = 12, FatsPer100g = 1.4m, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },
            new() { Name = "Sweet Potato", Category = FoodCategory.Carbohydrates, CaloriesPer100g = 86, ProteinPer100g = 1.6m, CarbsPer100g = 20, FatsPer100g = 0.1m, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },
            new() { Name = "Whole Wheat Bread", Category = FoodCategory.Grains, CaloriesPer100g = 247, ProteinPer100g = 13, CarbsPer100g = 41, FatsPer100g = 3.4m, DefaultUnit = "slice", CreatedAt = DateTime.UtcNow },
            new() { Name = "Spinach", Category = FoodCategory.Vegetables, CaloriesPer100g = 23, ProteinPer100g = 2.9m, CarbsPer100g = 3.6m, FatsPer100g = 0.4m, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },
            new() { Name = "Broccoli", Category = FoodCategory.Vegetables, CaloriesPer100g = 34, ProteinPer100g = 2.8m, CarbsPer100g = 7, FatsPer100g = 0.4m, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },
            new() { Name = "Banana", Category = FoodCategory.Fruits, CaloriesPer100g = 89, ProteinPer100g = 1.1m, CarbsPer100g = 23, FatsPer100g = 0.3m, DefaultUnit = "piece", CreatedAt = DateTime.UtcNow },
            new() { Name = "Apple", Category = FoodCategory.Fruits, CaloriesPer100g = 52, ProteinPer100g = 0.3m, CarbsPer100g = 14, FatsPer100g = 0.2m, DefaultUnit = "piece", CreatedAt = DateTime.UtcNow },
            new() { Name = "Almonds", Category = FoodCategory.Fats, CaloriesPer100g = 579, ProteinPer100g = 21, CarbsPer100g = 22, FatsPer100g = 50, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },
            new() { Name = "Milk (whole)", Category = FoodCategory.Dairy, CaloriesPer100g = 61, ProteinPer100g = 3.2m, CarbsPer100g = 4.8m, FatsPer100g = 3.3m, DefaultUnit = "cup", CreatedAt = DateTime.UtcNow },
            new() { Name = "Almond Milk (unsweetened)", Category = FoodCategory.Beverages, CaloriesPer100g = 13, ProteinPer100g = 0.4m, CarbsPer100g = 0.3m, FatsPer100g = 1.1m, DefaultUnit = "cup", CreatedAt = DateTime.UtcNow },
        };
        Console.WriteLine($"Adding {popularFoods.Count} food items...");
        await _context.FoodItems.AddRangeAsync(popularFoods);
        await _context.SaveChangesAsync();
        var newCount = await _context.FoodItems.CountAsync();
        Console.WriteLine($"Successfully seeded {newCount} food items!");
    }
    private static FoodItemDto MapToDto(FoodItem food) => new()
    {
        Id = food.Id,
        Name = food.Name,
        Category = food.Category.ToString(),
        CaloriesPer100g = food.CaloriesPer100g,
        ProteinPer100g = food.ProteinPer100g,
        CarbsPer100g = food.CarbsPer100g,
        FatsPer100g = food.FatsPer100g,
        DefaultUnit = food.DefaultUnit
    };
}
