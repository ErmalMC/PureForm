using Microsoft.AspNetCore.Mvc;
using PureForm.Application.Interfaces;

namespace PureForm.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FoodItemsController : ControllerBase
{
    private readonly IFoodItemService _foodItemService;

    public FoodItemsController(IFoodItemService foodItemService)
    {
        _foodItemService = foodItemService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var foods = await _foodItemService.GetAllAsync();
        return Ok(foods);
    }

    [HttpGet("popular")]
    public async Task<IActionResult> GetPopular()
    {
        var foods = await _foodItemService.GetPopularAsync();
        return Ok(foods);
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string query)
    {
        var foods = await _foodItemService.SearchAsync(query);
        return Ok(foods);
    }

    [HttpGet("category/{category}")]
    public async Task<IActionResult> GetByCategory(string category)
    {
        var foods = await _foodItemService.GetByCategoryAsync(category);
        return Ok(foods);
    }

    [HttpPost("seed")]
    public async Task<IActionResult> SeedFoods()
    {
        await _foodItemService.SeedPopularFoodsAsync();
        return Ok(new { message = "Popular foods seeded successfully" });
    }
}