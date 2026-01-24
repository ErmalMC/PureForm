using Microsoft.AspNetCore.Mvc;
using PureForm.Application.DTOs;
using PureForm.Application.Interfaces;

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
    public async Task<ActionResult<IEnumerable<FoodItemDto>>> GetAll()
    {
        var foods = await _foodItemService.GetAllAsync();
        return Ok(foods);
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<FoodItemDto>>> Search([FromQuery] string query)
    {
        var foods = await _foodItemService.SearchAsync(query);
        return Ok(foods);
    }

    [HttpGet("category/{category}")]
    public async Task<ActionResult<IEnumerable<FoodItemDto>>> GetByCategory(string category)
    {
        var foods = await _foodItemService.GetByCategoryAsync(category);
        return Ok(foods);
    }

    
}