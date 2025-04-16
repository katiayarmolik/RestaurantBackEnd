using Microsoft.AspNetCore.Mvc;
using RestaurantApi.Entities;
using RestaurantApi.Models;
using RestaurantApi.Services;
using ErrorOr;

namespace RestaurantApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DishController : ControllerBase
{
    private readonly IDishService _dishService;

    public DishController(IDishService dishService)
    {
        _dishService = dishService;
    }

    [HttpGet]
    [Route("")]
    public async Task<ActionResult<List<DishResponse>>> GetAllDishes()
    {
        var result = await _dishService.GetAllDishesAsync();
        
        return result.Match(
            dishes => Ok(dishes.Select(d => new DishResponse(d.Id, d.Name, d.Price))),
            errors => Problem(statusCode: StatusCodes.Status500InternalServerError, detail: errors.First().Description)
        );
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<DishResponse>> GetDish(int id)
    {
        var result = await _dishService.GetDishByIdAsync(id);
        
        return result.Match(
            dish => Ok(new DishResponse(dish.Id, dish.Name, dish.Price)),
            errors => Problem(statusCode: StatusCodes.Status404NotFound, detail: errors.First().Description)
        );
    }

    [HttpPost]
    [Route("")]
    public async Task<ActionResult<DishResponse>> CreateDish(CreateDishRequest request)
    {
        var dish = new Dish
        {
            Name = request.Name,
            Price = request.Price
        };

        var result = await _dishService.CreateDishAsync(dish);
        
        return result.Match(
            dish => CreatedAtAction(nameof(GetDish), new { id = dish.Id }, new DishResponse(dish.Id, dish.Name, dish.Price)),
            errors => Problem(statusCode: StatusCodes.Status400BadRequest, detail: errors.First().Description)
        );
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<ActionResult<DishResponse>> UpdateDish(int id, UpdateDishRequest request)
    {
        var dish = new Dish
        {
            Id = id,
            Name = request.Name,
            Price = request.Price
        };

        var result = await _dishService.UpdateDishAsync(dish);
        
        return result.Match(
            dish => Ok(new DishResponse(dish.Id, dish.Name, dish.Price)),
            errors => Problem(statusCode: StatusCodes.Status404NotFound, detail: errors.First().Description)
        );
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> DeleteDish(int id)
    {
        var result = await _dishService.DeleteDishAsync(id);
        
        return result.Match<IActionResult>(
            deleted => NoContent(),
            errors => Problem(statusCode: StatusCodes.Status404NotFound, detail: errors.First().Description)
        );
    }
} 