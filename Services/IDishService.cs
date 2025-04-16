using ErrorOr;
using RestaurantApi.Entities;

namespace RestaurantApi.Services;

public interface IDishService
{
    Task<ErrorOr<List<Dish>>> GetAllDishesAsync(CancellationToken cancellationToken = default);
    Task<ErrorOr<Dish>> GetDishByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ErrorOr<Dish>> CreateDishAsync(Dish dish, CancellationToken cancellationToken = default);
    Task<ErrorOr<Dish>> UpdateDishAsync(Dish dish, CancellationToken cancellationToken = default);
    Task<ErrorOr<Deleted>> DeleteDishAsync(int id, CancellationToken cancellationToken = default);
} 