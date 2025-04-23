using Microsoft.AspNetCore.Mvc;
using RestaurantApi.Entities;
using RestaurantApi.Models;
using RestaurantApi.Services;
using ErrorOr;

namespace RestaurantApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllOrders(CancellationToken cancellationToken)
    {
        var result = await _orderService.GetAllOrdersAsync(cancellationToken);
        return result.Match(
            orders => Ok(orders),
            errors => Problem(errors)
        );
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderById(int id, CancellationToken cancellationToken)
    {
        var result = await _orderService.GetOrderByIdAsync(id, cancellationToken);
        return result.Match(
            order => Ok(order),
            errors => Problem(errors)
        );
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request, CancellationToken cancellationToken)
    {
        var order = new Order
        {
            Waiter = request.Waiter
        };

        var result = await _orderService.CreateOrderAsync(order, cancellationToken);
        return result.Match(
            createdOrder => CreatedAtAction(nameof(GetOrderById), new { id = createdOrder.Id }, createdOrder),
            errors => Problem(errors)
        );
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOrder(int id, [FromBody] UpdateOrderRequest request, CancellationToken cancellationToken)
    {
        var order = new Order
        {
            Id = id,
            Waiter = request.Waiter
        };

        var result = await _orderService.UpdateOrderAsync(id, order, cancellationToken);
        return result.Match(
            updatedOrder => Ok(updatedOrder),
            errors => Problem(errors)
        );
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(int id, CancellationToken cancellationToken)
    {
        var result = await _orderService.DeleteOrderAsync(id, cancellationToken);
        return result.Match(
            _ => NoContent(),
            errors => Problem(errors)
        );
    }

    [HttpPost("{orderId}/dishes/{dishId}")]
    public async Task<IActionResult> AddDishToOrder(int orderId, int dishId, CancellationToken cancellationToken)
    {
        var result = await _orderService.AddDishToOrderAsync(orderId, dishId, cancellationToken);
        return result.Match(
            order => Ok(order),
            errors => Problem(errors)
        );
    }

    [HttpDelete("{orderId}/dishes/{dishId}")]
    public async Task<IActionResult> RemoveDishFromOrder(int orderId, int dishId, CancellationToken cancellationToken)
    {
        var result = await _orderService.RemoveDishFromOrderAsync(orderId, dishId, cancellationToken);
        return result.Match(
            order => Ok(order),
            errors => Problem(errors)
        );
    }
} 