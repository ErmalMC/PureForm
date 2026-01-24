    namespace PureForm.Application.Interfaces;
using PureForm.Application.DTOs;

public interface IFoodItemService
{
    Task<IEnumerable<FoodItemDto>> GetAllAsync();
    Task<IEnumerable<FoodItemDto>> SearchAsync(string query);
    Task<IEnumerable<FoodItemDto>> GetByCategoryAsync(string category);
    Task SeedPopularFoodsAsync();
    
}