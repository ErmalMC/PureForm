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
        // ========== PROTEINS (Meat & Poultry) ==========
        new() { Name = "Chicken Breast", Category = "Protein", CaloriesPer100g = 165, ProteinPer100g = 31, CarbsPer100g = 0, FatsPer100g = 3.6m, IsPopular = true, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },
        new() { Name = "Chicken Thigh", Category = "Protein", CaloriesPer100g = 209, ProteinPer100g = 26, CarbsPer100g = 0, FatsPer100g = 11, IsPopular = true, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },
        new() { Name = "Turkey Breast", Category = "Protein", CaloriesPer100g = 135, ProteinPer100g = 30, CarbsPer100g = 0, FatsPer100g = 1, IsPopular = true, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },
        new() { Name = "Ground Beef (90% lean)", Category = "Protein", CaloriesPer100g = 176, ProteinPer100g = 20, CarbsPer100g = 0, FatsPer100g = 10, IsPopular = true, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },
        new() { Name = "Ground Beef (80% lean)", Category = "Protein", CaloriesPer100g = 254, ProteinPer100g = 17, CarbsPer100g = 0, FatsPer100g = 20, IsPopular = false, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },
        new() { Name = "Steak (Sirloin)", Category = "Protein", CaloriesPer100g = 271, ProteinPer100g = 27, CarbsPer100g = 0, FatsPer100g = 18, IsPopular = true, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },
        new() { Name = "Pork Chop", Category = "Protein", CaloriesPer100g = 231, ProteinPer100g = 25, CarbsPer100g = 0, FatsPer100g = 14, IsPopular = false, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },
        new() { Name = "Bacon", Category = "Protein", CaloriesPer100g = 541, ProteinPer100g = 37, CarbsPer100g = 1.4m, FatsPer100g = 42, IsPopular = true, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },

        // ========== PROTEINS (Seafood) ==========
        new() { Name = "Salmon", Category = "Protein", CaloriesPer100g = 208, ProteinPer100g = 20, CarbsPer100g = 0, FatsPer100g = 13, IsPopular = true, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },
        new() { Name = "Tuna (canned in water)", Category = "Protein", CaloriesPer100g = 116, ProteinPer100g = 26, CarbsPer100g = 0, FatsPer100g = 1, IsPopular = true, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },
        new() { Name = "Tuna Steak", Category = "Protein", CaloriesPer100g = 144, ProteinPer100g = 23, CarbsPer100g = 0, FatsPer100g = 5, IsPopular = false, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },
        new() { Name = "Shrimp", Category = "Protein", CaloriesPer100g = 99, ProteinPer100g = 24, CarbsPer100g = 0.2m, FatsPer100g = 0.3m, IsPopular = true, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },
        new() { Name = "Tilapia", Category = "Protein", CaloriesPer100g = 128, ProteinPer100g = 26, CarbsPer100g = 0, FatsPer100g = 3, IsPopular = false, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },
        new() { Name = "Cod", Category = "Protein", CaloriesPer100g = 82, ProteinPer100g = 18, CarbsPer100g = 0, FatsPer100g = 0.7m, IsPopular = false, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },

        // ========== PROTEINS (Eggs & Dairy Proteins) ==========
        new() { Name = "Eggs (whole)", Category = "Protein", CaloriesPer100g = 143, ProteinPer100g = 13, CarbsPer100g = 1, FatsPer100g = 10, IsPopular = true, DefaultUnit = "piece", CreatedAt = DateTime.UtcNow },
        new() { Name = "Egg Whites", Category = "Protein", CaloriesPer100g = 52, ProteinPer100g = 11, CarbsPer100g = 0.7m, FatsPer100g = 0.2m, IsPopular = true, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },
        new() { Name = "Greek Yogurt (non-fat)", Category = "Protein", CaloriesPer100g = 59, ProteinPer100g = 10, CarbsPer100g = 3.6m, FatsPer100g = 0.4m, IsPopular = true, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },
        new() { Name = "Greek Yogurt (full-fat)", Category = "Protein", CaloriesPer100g = 97, ProteinPer100g = 9, CarbsPer100g = 3.6m, FatsPer100g = 5, IsPopular = true, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },
        new() { Name = "Cottage Cheese", Category = "Protein", CaloriesPer100g = 98, ProteinPer100g = 11, CarbsPer100g = 3.4m, FatsPer100g = 4.3m, IsPopular = true, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },

        // ========== PROTEINS (Plant-Based) ==========
        new() { Name = "Tofu", Category = "Protein", CaloriesPer100g = 76, ProteinPer100g = 8, CarbsPer100g = 1.9m, FatsPer100g = 4.8m, IsPopular = true, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },
        new() { Name = "Tempeh", Category = "Protein", CaloriesPer100g = 193, ProteinPer100g = 20, CarbsPer100g = 9, FatsPer100g = 11, IsPopular = false, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },
        new() { Name = "Lentils (cooked)", Category = "Protein", CaloriesPer100g = 116, ProteinPer100g = 9, CarbsPer100g = 20, FatsPer100g = 0.4m, IsPopular = true, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },
        new() { Name = "Black Beans (cooked)", Category = "Protein", CaloriesPer100g = 132, ProteinPer100g = 9, CarbsPer100g = 24, FatsPer100g = 0.5m, IsPopular = true, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },
        new() { Name = "Chickpeas (cooked)", Category = "Protein", CaloriesPer100g = 164, ProteinPer100g = 9, CarbsPer100g = 27, FatsPer100g = 2.6m, IsPopular = true, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },
        new() { Name = "Protein Powder (Whey)", Category = "Protein", CaloriesPer100g = 412, ProteinPer100g = 82, CarbsPer100g = 6, FatsPer100g = 8, IsPopular = true, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },

        // ========== CARBS (Grains & Starches) ==========
        new() { Name = "Brown Rice (cooked)", Category = "Carbs", CaloriesPer100g = 111, ProteinPer100g = 2.6m, CarbsPer100g = 23, FatsPer100g = 0.9m, IsPopular = true, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },
        new() { Name = "White Rice (cooked)", Category = "Carbs", CaloriesPer100g = 130, ProteinPer100g = 2.7m, CarbsPer100g = 28, FatsPer100g = 0.3m, IsPopular = true, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },
        new() { Name = "Jasmine Rice (cooked)", Category = "Carbs", CaloriesPer100g = 129, ProteinPer100g = 2.7m, CarbsPer100g = 28, FatsPer100g = 0.2m, IsPopular = false, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },
        new() { Name = "Basmati Rice (cooked)", Category = "Carbs", CaloriesPer100g = 121, ProteinPer100g = 2.5m, CarbsPer100g = 25, FatsPer100g = 0.4m, IsPopular = false, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },
        new() { Name = "Oatmeal (dry)", Category = "Carbs", CaloriesPer100g = 389, ProteinPer100g = 17, CarbsPer100g = 66, FatsPer100g = 7, IsPopular = true, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },
        new() { Name = "Oatmeal (cooked)", Category = "Carbs", CaloriesPer100g = 71, ProteinPer100g = 2.5m, CarbsPer100g = 12, FatsPer100g = 1.4m, IsPopular = true, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },
        new() { Name = "Quinoa (cooked)", Category = "Carbs", CaloriesPer100g = 120, ProteinPer100g = 4.4m, CarbsPer100g = 21, FatsPer100g = 1.9m, IsPopular = true, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },
        new() { Name = "Pasta (cooked)", Category = "Carbs", CaloriesPer100g = 131, ProteinPer100g = 5, CarbsPer100g = 25, FatsPer100g = 1.1m, IsPopular = true, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },
        new() { Name = "Whole Wheat Pasta (cooked)", Category = "Carbs", CaloriesPer100g = 124, ProteinPer100g = 5, CarbsPer100g = 26, FatsPer100g = 0.5m, IsPopular = true, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },
        new() { Name = "Couscous (cooked)", Category = "Carbs", CaloriesPer100g = 112, ProteinPer100g = 3.8m, CarbsPer100g = 23, FatsPer100g = 0.2m, IsPopular = false, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },

        // ========== CARBS (Bread & Baked Goods) ==========
        new() { Name = "Whole Wheat Bread", Category = "Carbs", CaloriesPer100g = 247, ProteinPer100g = 13, CarbsPer100g = 41, FatsPer100g = 3.4m, IsPopular = true, DefaultUnit = "slice", CreatedAt = DateTime.UtcNow },
        new() { Name = "White Bread", Category = "Carbs", CaloriesPer100g = 265, ProteinPer100g = 9, CarbsPer100g = 49, FatsPer100g = 3.2m, IsPopular = true, DefaultUnit = "slice", CreatedAt = DateTime.UtcNow },
        new() { Name = "Bagel", Category = "Carbs", CaloriesPer100g = 257, ProteinPer100g = 10, CarbsPer100g = 50, FatsPer100g = 1.5m, IsPopular = true, DefaultUnit = "piece", CreatedAt = DateTime.UtcNow },
        new() { Name = "English Muffin", Category = "Carbs", CaloriesPer100g = 235, ProteinPer100g = 7.6m, CarbsPer100g = 46, FatsPer100g = 2, IsPopular = false, DefaultUnit = "piece", CreatedAt = DateTime.UtcNow },
        new() { Name = "Tortilla (flour)", Category = "Carbs", CaloriesPer100g = 304, ProteinPer100g = 8, CarbsPer100g = 51, FatsPer100g = 7, IsPopular = true, DefaultUnit = "piece", CreatedAt = DateTime.UtcNow },
        new() { Name = "Pita Bread", Category = "Carbs", CaloriesPer100g = 275, ProteinPer100g = 9, CarbsPer100g = 55, FatsPer100g = 1.2m, IsPopular = false, DefaultUnit = "piece", CreatedAt = DateTime.UtcNow },

        // ========== CARBS (Potatoes & Root Vegetables) ==========
        new() { Name = "Sweet Potato", Category = "Carbs", CaloriesPer100g = 86, ProteinPer100g = 1.6m, CarbsPer100g = 20, FatsPer100g = 0.1m, IsPopular = true, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },
        new() { Name = "Potato (baked)", Category = "Carbs", CaloriesPer100g = 93, ProteinPer100g = 2.5m, CarbsPer100g = 21, FatsPer100g = 0.1m, IsPopular = true, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },
        new() { Name = "French Fries", Category = "Carbs", CaloriesPer100g = 312, ProteinPer100g = 3.4m, CarbsPer100g = 41, FatsPer100g = 15, IsPopular = true, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },
        new() { Name = "Mashed Potatoes", Category = "Carbs", CaloriesPer100g = 116, ProteinPer100g = 2, CarbsPer100g = 17, FatsPer100g = 4.2m, IsPopular = false, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },

        // ========== VEGETABLES (Leafy Greens) ==========
        new() { Name = "Spinach", Category = "Vegetables", CaloriesPer100g = 23, ProteinPer100g = 2.9m, CarbsPer100g = 3.6m, FatsPer100g = 0.4m, IsPopular = true, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },
        new() { Name = "Kale", Category = "Vegetables", CaloriesPer100g = 35, ProteinPer100g = 2.9m, CarbsPer100g = 4.4m, FatsPer100g = 1.5m, IsPopular = true, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },
        new() { Name = "Lettuce (Romaine)", Category = "Vegetables", CaloriesPer100g = 17, ProteinPer100g = 1.2m, CarbsPer100g = 3.3m, FatsPer100g = 0.3m, IsPopular = true, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },
        new() { Name = "Arugula", Category = "Vegetables", CaloriesPer100g = 25, ProteinPer100g = 2.6m, CarbsPer100g = 3.7m, FatsPer100g = 0.7m, IsPopular = false, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },

        // ========== VEGETABLES (Cruciferous & Others) ==========
        new() { Name = "Broccoli", Category = "Vegetables", CaloriesPer100g = 34, ProteinPer100g = 2.8m, CarbsPer100g = 7, FatsPer100g = 0.4m, IsPopular = true, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },
        new() { Name = "Cauliflower", Category = "Vegetables", CaloriesPer100g = 25, ProteinPer100g = 1.9m, CarbsPer100g = 5, FatsPer100g = 0.3m, IsPopular = true, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },
        new() { Name = "Brussels Sprouts", Category = "Vegetables", CaloriesPer100g = 43, ProteinPer100g = 3.4m, CarbsPer100g = 9, FatsPer100g = 0.3m, IsPopular = false, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },
        new() { Name = "Asparagus", Category = "Vegetables", CaloriesPer100g = 20, ProteinPer100g = 2.2m, CarbsPer100g = 3.9m, FatsPer100g = 0.1m, IsPopular = true, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },
        new() { Name = "Green Beans", Category = "Vegetables", CaloriesPer100g = 31, ProteinPer100g = 1.8m, CarbsPer100g = 7, FatsPer100g = 0.2m, IsPopular = true, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },
        new() { Name = "Zucchini", Category = "Vegetables", CaloriesPer100g = 17, ProteinPer100g = 1.2m, CarbsPer100g = 3.1m, FatsPer100g = 0.3m, IsPopular = true, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },
        new() { Name = "Bell Peppers (red)", Category = "Vegetables", CaloriesPer100g = 31, ProteinPer100g = 1, CarbsPer100g = 6, FatsPer100g = 0.3m, IsPopular = true, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },
        new() { Name = "Bell Peppers (green)", Category = "Vegetables", CaloriesPer100g = 20, ProteinPer100g = 0.9m, CarbsPer100g = 4.6m, FatsPer100g = 0.2m, IsPopular = true, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },
        new() { Name = "Carrots", Category = "Vegetables", CaloriesPer100g = 41, ProteinPer100g = 0.9m, CarbsPer100g = 10, FatsPer100g = 0.2m, IsPopular = true, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },
        new() { Name = "Cucumber", Category = "Vegetables", CaloriesPer100g = 15, ProteinPer100g = 0.7m, CarbsPer100g = 3.6m, FatsPer100g = 0.1m, IsPopular = true, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },
        new() { Name = "Tomato", Category = "Vegetables", CaloriesPer100g = 18, ProteinPer100g = 0.9m, CarbsPer100g = 3.9m, FatsPer100g = 0.2m, IsPopular = true, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },
        new() { Name = "Cherry Tomatoes", Category = "Vegetables", CaloriesPer100g = 18, ProteinPer100g = 0.9m, CarbsPer100g = 3.9m, FatsPer100g = 0.2m, IsPopular = true, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },
        new() { Name = "Onion", Category = "Vegetables", CaloriesPer100g = 40, ProteinPer100g = 1.1m, CarbsPer100g = 9, FatsPer100g = 0.1m, IsPopular = true, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },
        new() { Name = "Mushrooms", Category = "Vegetables", CaloriesPer100g = 22, ProteinPer100g = 3.1m, CarbsPer100g = 3.3m, FatsPer100g = 0.3m, IsPopular = true, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },

        // ========== FRUITS (Berries) ==========
        new() { Name = "Blueberries", Category = "Fruits", CaloriesPer100g = 57, ProteinPer100g = 0.7m, CarbsPer100g = 14, FatsPer100g = 0.3m, IsPopular = true, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },
        new() { Name = "Strawberries", Category = "Fruits", CaloriesPer100g = 32, ProteinPer100g = 0.7m, CarbsPer100g = 7.7m, FatsPer100g = 0.3m, IsPopular = true, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },
        new() { Name = "Raspberries", Category = "Fruits", CaloriesPer100g = 52, ProteinPer100g = 1.2m, CarbsPer100g = 12, FatsPer100g = 0.7m, IsPopular = true, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },
        new() { Name = "Blackberries", Category = "Fruits", CaloriesPer100g = 43, ProteinPer100g = 1.4m, CarbsPer100g = 10, FatsPer100g = 0.5m, IsPopular = false, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },

        // ========== FRUITS (Common Fruits) ==========
        new() { Name = "Banana", Category = "Fruits", CaloriesPer100g = 89, ProteinPer100g = 1.1m, CarbsPer100g = 23, FatsPer100g = 0.3m, IsPopular = true, DefaultUnit = "piece", CreatedAt = DateTime.UtcNow },
        new() { Name = "Apple", Category = "Fruits", CaloriesPer100g = 52, ProteinPer100g = 0.3m, CarbsPer100g = 14, FatsPer100g = 0.2m, IsPopular = true, DefaultUnit = "piece", CreatedAt = DateTime.UtcNow },
        new() { Name = "Orange", Category = "Fruits", CaloriesPer100g = 47, ProteinPer100g = 0.9m, CarbsPer100g = 12, FatsPer100g = 0.1m, IsPopular = true, DefaultUnit = "piece", CreatedAt = DateTime.UtcNow },
        new() { Name = "Grapes", Category = "Fruits", CaloriesPer100g = 69, ProteinPer100g = 0.7m, CarbsPer100g = 18, FatsPer100g = 0.2m, IsPopular = true, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },
        new() { Name = "Watermelon", Category = "Fruits", CaloriesPer100g = 30, ProteinPer100g = 0.6m, CarbsPer100g = 8, FatsPer100g = 0.2m, IsPopular = true, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },
        new() { Name = "Pineapple", Category = "Fruits", CaloriesPer100g = 50, ProteinPer100g = 0.5m, CarbsPer100g = 13, FatsPer100g = 0.1m, IsPopular = true, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },
        new() { Name = "Mango", Category = "Fruits", CaloriesPer100g = 60, ProteinPer100g = 0.8m, CarbsPer100g = 15, FatsPer100g = 0.4m, IsPopular = true, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },
        new() { Name = "Peach", Category = "Fruits", CaloriesPer100g = 39, ProteinPer100g = 0.9m, CarbsPer100g = 10, FatsPer100g = 0.3m, IsPopular = false, DefaultUnit = "piece", CreatedAt = DateTime.UtcNow },
        new() { Name = "Pear", Category = "Fruits", CaloriesPer100g = 57, ProteinPer100g = 0.4m, CarbsPer100g = 15, FatsPer100g = 0.1m, IsPopular = false, DefaultUnit = "piece", CreatedAt = DateTime.UtcNow },
        new() { Name = "Kiwi", Category = "Fruits", CaloriesPer100g = 61, ProteinPer100g = 1.1m, CarbsPer100g = 15, FatsPer100g = 0.5m, IsPopular = false, DefaultUnit = "piece", CreatedAt = DateTime.UtcNow },

        // ========== HEALTHY FATS (Nuts & Seeds) ==========
        new() { Name = "Almonds", Category = "Fats", CaloriesPer100g = 579, ProteinPer100g = 21, CarbsPer100g = 22, FatsPer100g = 50, IsPopular = true, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },
        new() { Name = "Walnuts", Category = "Fats", CaloriesPer100g = 654, ProteinPer100g = 15, CarbsPer100g = 14, FatsPer100g = 65, IsPopular = true, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },
        new() { Name = "Cashews", Category = "Fats", CaloriesPer100g = 553, ProteinPer100g = 18, CarbsPer100g = 30, FatsPer100g = 44, IsPopular = true, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },
        new() { Name = "Peanuts", Category = "Fats", CaloriesPer100g = 567, ProteinPer100g = 26, CarbsPer100g = 16, FatsPer100g = 49, IsPopular = true, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },
        new() { Name = "Peanut Butter", Category = "Fats", CaloriesPer100g = 588, ProteinPer100g = 25, CarbsPer100g = 20, FatsPer100g = 50, IsPopular = true, DefaultUnit = "tbsp", CreatedAt = DateTime.UtcNow },
        new() { Name = "Almond Butter", Category = "Fats", CaloriesPer100g = 614, ProteinPer100g = 21, CarbsPer100g = 18, FatsPer100g = 56, IsPopular = true, DefaultUnit = "tbsp", CreatedAt = DateTime.UtcNow },
        new() { Name = "Chia Seeds", Category = "Fats", CaloriesPer100g = 486, ProteinPer100g = 17, CarbsPer100g = 42, FatsPer100g = 31, IsPopular = true, DefaultUnit = "tbsp", CreatedAt = DateTime.UtcNow },
        new() { Name = "Flax Seeds", Category = "Fats", CaloriesPer100g = 534, ProteinPer100g = 18, CarbsPer100g = 29, FatsPer100g = 42, IsPopular = false, DefaultUnit = "tbsp", CreatedAt = DateTime.UtcNow },
        new() { Name = "Sunflower Seeds", Category = "Fats", CaloriesPer100g = 584, ProteinPer100g = 21, CarbsPer100g = 20, FatsPer100g = 51, IsPopular = false, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },

        // ========== HEALTHY FATS (Oils & Others) ==========
        new() { Name = "Avocado", Category = "Fats", CaloriesPer100g = 160, ProteinPer100g = 2, CarbsPer100g = 9, FatsPer100g = 15, IsPopular = true, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },
        new() { Name = "Olive Oil", Category = "Fats", CaloriesPer100g =884, ProteinPer100g = 0, CarbsPer100g = 0, FatsPer100g = 100, IsPopular = true, DefaultUnit = "tbsp", CreatedAt = DateTime.UtcNow },
        new() { Name = "Coconut Oil", Category = "Fats", CaloriesPer100g = 892, ProteinPer100g = 0, CarbsPer100g = 0, FatsPer100g = 99, IsPopular = false, DefaultUnit = "tbsp", CreatedAt = DateTime.UtcNow },
        new() { Name = "Butter", Category = "Fats", CaloriesPer100g = 717, ProteinPer100g = 0.9m, CarbsPer100g = 0.1m, FatsPer100g = 81, IsPopular = true, DefaultUnit = "tbsp", CreatedAt = DateTime.UtcNow },
        // ========== DAIRY ==========
    new() { Name = "Milk (whole)", Category = "Dairy", CaloriesPer100g = 61, ProteinPer100g = 3.2m, CarbsPer100g = 4.8m, FatsPer100g = 3.3m, IsPopular = true, DefaultUnit = "cup", CreatedAt = DateTime.UtcNow },
    new() { Name = "Milk (2%)", Category = "Dairy", CaloriesPer100g = 50, ProteinPer100g = 3.3m, CarbsPer100g = 4.7m, FatsPer100g = 2, IsPopular = true, DefaultUnit = "cup", CreatedAt = DateTime.UtcNow },
    new() { Name = "Milk (skim)", Category = "Dairy", CaloriesPer100g = 34, ProteinPer100g = 3.4m, CarbsPer100g = 5, FatsPer100g = 0.1m, IsPopular = true, DefaultUnit = "cup", CreatedAt = DateTime.UtcNow },
    new() { Name = "Almond Milk (unsweetened)", Category = "Dairy", CaloriesPer100g = 13, ProteinPer100g = 0.4m, CarbsPer100g = 0.3m, FatsPer100g = 1.1m, IsPopular = true, DefaultUnit = "cup", CreatedAt = DateTime.UtcNow },
    new() { Name = "Oat Milk", Category = "Dairy", CaloriesPer100g = 47, ProteinPer100g = 1, CarbsPer100g = 7.6m, FatsPer100g = 1.5m, IsPopular = true, DefaultUnit = "cup", CreatedAt = DateTime.UtcNow },
    new() { Name = "Cheddar Cheese", Category = "Dairy", CaloriesPer100g = 403, ProteinPer100g = 25, CarbsPer100g = 1.3m, FatsPer100g = 33, IsPopular = true, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },
    new() { Name = "Mozzarella Cheese", Category = "Dairy", CaloriesPer100g = 280, ProteinPer100g = 28, CarbsPer100g = 2.2m, FatsPer100g = 17, IsPopular = true, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },
    new() { Name = "Parmesan Cheese", Category = "Dairy", CaloriesPer100g = 431, ProteinPer100g = 38, CarbsPer100g = 4.1m, FatsPer100g = 29, IsPopular = true, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },
    new() { Name = "Feta Cheese", Category = "Dairy", CaloriesPer100g = 264, ProteinPer100g = 14, CarbsPer100g = 4.1m, FatsPer100g = 21, IsPopular = false, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },
    new() { Name = "Cream Cheese", Category = "Dairy", CaloriesPer100g = 342, ProteinPer100g = 6, CarbsPer100g = 4.1m, FatsPer100g = 34, IsPopular = true, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },

    // ========== SNACKS & EXTRAS ==========
    new() { Name = "Dark Chocolate (70%)", Category = "Snacks", CaloriesPer100g = 598, ProteinPer100g = 8, CarbsPer100g = 46, FatsPer100g = 43, IsPopular = true, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },
    new() { Name = "Popcorn (air-popped)", Category = "Snacks", CaloriesPer100g = 387, ProteinPer100g = 13, CarbsPer100g = 78, FatsPer100g = 4.5m, IsPopular = true, DefaultUnit = "cup", CreatedAt = DateTime.UtcNow },
    new() { Name = "Granola", Category = "Snacks", CaloriesPer100g = 471, ProteinPer100g = 13, CarbsPer100g = 64, FatsPer100g = 20, IsPopular = true, DefaultUnit = "g", CreatedAt = DateTime.UtcNow },
    new() { Name = "Hummus", Category = "Snacks", CaloriesPer100g = 177, ProteinPer100g = 8, CarbsPer100g = 14, FatsPer100g = 10, IsPopular = true, DefaultUnit = "tbsp", CreatedAt = DateTime.UtcNow },
    new() { Name = "Protein Bar", Category = "Snacks", CaloriesPer100g = 400, ProteinPer100g = 20, CarbsPer100g = 40, FatsPer100g = 15, IsPopular = true, DefaultUnit = "piece", CreatedAt = DateTime.UtcNow },
    new() { Name = "Rice Cakes", Category = "Snacks", CaloriesPer100g = 387, ProteinPer100g = 8, CarbsPer100g = 82, FatsPer100g = 3, IsPopular = false, DefaultUnit = "piece", CreatedAt = DateTime.UtcNow },
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