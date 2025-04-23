using ErrorOr;
using Models;

namespace Services;

public interface IOrderService
{
    Task<ErrorOr<List<Order>>> GetAllOrdersAsync(CancellationToken cancellationToken = default);
    Task<ErrorOr<Order>> GetOrderByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ErrorOr<Order>> CreateOrderAsync(Order order, CancellationToken cancellationToken = default);
    Task<ErrorOr<Order>> UpdateOrderAsync(int id, Order order, CancellationToken cancellationToken = default);
    Task<ErrorOr<Order>> DeleteOrderAsync(int id, CancellationToken cancellationToken = default);
    
    // Additional methods for managing order dishes
    Task<ErrorOr<Order>> AddDishToOrderAsync(int orderId, int dishId, CancellationToken cancellationToken = default);
    Task<ErrorOr<Order>> RemoveDishFromOrderAsync(int orderId, int dishId, CancellationToken cancellationToken = default);
} 