using ErrorOr;
using RestaurantApi.Entities;

namespace RestaurantApi.Services;

public interface IOrderService
{
    Task<ErrorOr<List<Order>>> GetAllOrdersAsync(CancellationToken cancellationToken = default);
    Task<ErrorOr<Order>> GetOrderByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ErrorOr<Order>> CreateOrderAsync(Order order, CancellationToken cancellationToken = default);
    Task<ErrorOr<Order>> UpdateOrderAsync(Order order, CancellationToken cancellationToken = default);
    Task<ErrorOr<Deleted>> DeleteOrderAsync(int id, CancellationToken cancellationToken = default);
    
    // Additional methods for managing order dishes
    Task<ErrorOr<Order>> AddDishToOrderAsync(int orderId, int dishId, CancellationToken cancellationToken = default);
    Task<ErrorOr<Order>> RemoveDishFromOrderAsync(int orderId, int dishId, CancellationToken cancellationToken = default);
} 