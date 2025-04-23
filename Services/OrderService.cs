using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Models;
using RestaurantApi.Data;
using RestaurantApi.Entities;

namespace RestaurantApi.Services;

public class OrderService : IOrderService
{
    private readonly RestaurantDbContext _context;

    public OrderService(RestaurantDbContext context)
    {
        _context = context;
    }

    public async Task<ErrorOr<List<Order>>> GetAllOrdersAsync(CancellationToken cancellationToken = default)
    {
        var orders = await _context.Orders
            .Include(o => o.OrderDishes)
            .ThenInclude(od => od.Dish)
            .ToListAsync(cancellationToken);
        return orders;
    }

    public async Task<ErrorOr<Order>> GetOrderByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var order = await _context.Orders
            .Include(o => o.OrderDishes)
            .ThenInclude(od => od.Dish)
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
        
        if (order is null)
        {
            return Error.NotFound("Order not found");
        }
        
        return order;
    }

    public async Task<ErrorOr<Order>> CreateOrderAsync(Order order, CancellationToken cancellationToken = default)
    {
        order.CreatedAt = DateTime.UtcNow;
        _context.Orders.Add(order);
        await _context.SaveChangesAsync(cancellationToken);
        return order;
    }

    public async Task<ErrorOr<Order>> UpdateOrderAsync(int id, Order order, CancellationToken cancellationToken = default)
    {
        var existingOrder = await _context.Orders.FindAsync(new object[] { id }, cancellationToken);
        if (existingOrder is null)
        {
            return Error.NotFound("Order not found");
        }

        existingOrder.Waiter = order.Waiter;
        await _context.SaveChangesAsync(cancellationToken);
        return existingOrder;
    }

    public async Task<ErrorOr<Order>> DeleteOrderAsync(int id, CancellationToken cancellationToken = default)
    {
        var order = await _context.Orders.FindAsync(new object[] { id }, cancellationToken);
        if (order is null)
        {
            return Error.NotFound("Order not found");
        }

        _context.Orders.Remove(order);
        await _context.SaveChangesAsync(cancellationToken);
        return order;
    }

    public async Task<ErrorOr<Order>> AddDishToOrderAsync(int orderId, int dishId, CancellationToken cancellationToken = default)
    {
        var order = await _context.Orders
            .Include(o => o.OrderDishes)
            .FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);
        
        if (order is null)
        {
            return Error.NotFound("Order not found");
        }

        var dish = await _context.Dishes.FindAsync(new object[] { dishId }, cancellationToken);
        if (dish is null)
        {
            return Error.NotFound("Dish not found");
        }

        if (order.OrderDishes.Any(od => od.DishId == dishId))
        {
            return Error.Conflict("Dish is already in the order");
        }

        order.OrderDishes.Add(new OrderDish { OrderId = orderId, DishId = dishId });
        await _context.SaveChangesAsync(cancellationToken);
        return order;
    }

    public async Task<ErrorOr<Order>> RemoveDishFromOrderAsync(int orderId, int dishId, CancellationToken cancellationToken = default)
    {
        var order = await _context.Orders
            .Include(o => o.OrderDishes)
            .FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);
        
        if (order is null)
        {
            return Error.NotFound("Order not found");
        }

        var orderDish = order.OrderDishes.FirstOrDefault(od => od.DishId == dishId);
        if (orderDish is null)
        {
            return Error.NotFound("Dish is not in the order");
        }

        order.OrderDishes.Remove(orderDish);
        await _context.SaveChangesAsync(cancellationToken);
        return order;
    }
} 