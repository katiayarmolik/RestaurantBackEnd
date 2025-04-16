using ErrorOr;
using Microsoft.EntityFrameworkCore;
using RestaurantApi.Data;
using RestaurantApi.Entities;

namespace RestaurantApi.Services;

public class DishService : IDishService
{
    private readonly RestaurantDbContext _context;

    public DishService(RestaurantDbContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<List<Dish>>> GetAllDishesAsync(CancellationToken cancellationToken = default)
    {
        var dishes = await _context.Dishes.ToListAsync(cancellationToken);
        return dishes;
    }

    public async Task<ErrorOr<Dish>> GetDishByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var dish = await _context.Dishes.FindAsync(new object[] { id }, cancellationToken);
        
        if (dish is null)
        {
            return Error.NotFound("Dish not found");
        }

        return dish;
    }

    public async Task<ErrorOr<Dish>> CreateDishAsync(Dish dish, CancellationToken cancellationToken = default)
    {
        _context.Dishes.Add(dish);
        await _context.SaveChangesAsync(cancellationToken);
        return dish;
    }

    public async Task<ErrorOr<Dish>> UpdateDishAsync(Dish dish, CancellationToken cancellationToken = default)
    {
        var existingDish = await _context.Dishes.FindAsync(new object[] { dish.Id }, cancellationToken);
        
        if (existingDish is null)
        {
            return Error.NotFound("Dish not found");
        }

        existingDish.Name = dish.Name;
        existingDish.Price = dish.Price;

        await _context.SaveChangesAsync(cancellationToken);
        return existingDish;
    }

    public async Task<ErrorOr<Deleted>> DeleteDishAsync(int id, CancellationToken cancellationToken = default)
    {
        var dish = await _context.Dishes.FindAsync(new object[] { id }, cancellationToken);
        
        if (dish is null)
        {
            return Error.NotFound("Dish not found");
        }

        _context.Dishes.Remove(dish);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Deleted;
    }
} 