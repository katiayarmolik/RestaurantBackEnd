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
    [Route("")]
    public async Task<ActionResult<List<OrderResponse>>> GetAllOrders()
    {
        var result = await _orderService.GetAllOrdersAsync();
        
        return result.Match(
            orders => Ok(orders.Select(o => new OrderResponse(
                o.Id,
                o.Waiter,
                o.CreatedAt,
                o.OrderDishes.Select(od => new OrderDishResponse(
                    od.Dish.Id,
                    od.Dish.Name,
                    od.Dish.Price
                )).ToList()
            ))),
            errors => Problem(statusCode: StatusCodes.Status500InternalServerError, detail: errors.First().Description)
        );
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<OrderResponse>> GetOrder(int id)
    {
        var result = await _orderService.GetOrderByIdAsync(id);
        
        return result.Match(
            order => Ok(new OrderResponse(
                order.Id,
                order.Waiter,
                order.CreatedAt,
                order.OrderDishes.Select(od => new OrderDishResponse(
                    od.Dish.Id,
                    od.Dish.Name,
                    od.Dish.Price
                )).ToList()
            )),
            errors => Problem(statusCode: StatusCodes.Status404NotFound, detail: errors.First().Description)
        );
    }

    [HttpPost]
    [Route("")]
    public async Task<ActionResult<OrderResponse>> CreateOrder(CreateOrderRequest request)
    {
        var order = new Order
        {
            Waiter = request.Waiter,
            CreatedAt = DateTime.UtcNow
        };

        var result = await _orderService.CreateOrderAsync(order);
        
        return result.Match(
            order => CreatedAtAction(nameof(GetOrder), new { id = order.Id }, new OrderResponse(
                order.Id,
                order.Waiter,
                order.CreatedAt,
                order.OrderDishes.Select(od => new OrderDishResponse(
                    od.Dish.Id,
                    od.Dish.Name,
                    od.Dish.Price
                )).ToList()
            )),
            errors => Problem(statusCode: StatusCodes.Status400BadRequest, detail: errors.First().Description)
        );
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<ActionResult<OrderResponse>> UpdateOrder(int id, UpdateOrderRequest request)
    {
        var order = new Order
        {
            Id = id,
            Waiter = request.Waiter,
            CreatedAt = DateTime.UtcNow
        };

        var result = await _orderService.UpdateOrderAsync(order);
        
        return result.Match(
            order => Ok(new OrderResponse(
                order.Id,
                order.Waiter,
                order.CreatedAt,
                order.OrderDishes.Select(od => new OrderDishResponse(
                    od.Dish.Id,
                    od.Dish.Name,
                    od.Dish.Price
                )).ToList()
            )),
            errors => Problem(statusCode: StatusCodes.Status404NotFound, detail: errors.First().Description)
        );
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        var result = await _orderService.DeleteOrderAsync(id);
        
        return result.Match<IActionResult>(
            deleted => NoContent(),
            errors => Problem(statusCode: StatusCodes.Status404NotFound, detail: errors.First().Description)
        );
    }

    [HttpPost]
    [Route("{orderId}/dishes/{dishId}")]
    public async Task<ActionResult<OrderResponse>> AddDishToOrder(int orderId, int dishId)
    {
        var result = await _orderService.AddDishToOrderAsync(orderId, dishId);
        
        return result.Match(
            order => Ok(new OrderResponse(
                order.Id,
                order.Waiter,
                order.CreatedAt,
                order.OrderDishes.Select(od => new OrderDishResponse(
                    od.Dish.Id,
                    od.Dish.Name,
                    od.Dish.Price
                )).ToList()
            )),
            errors => Problem(
                statusCode: errors.First().Type == ErrorType.NotFound ? StatusCodes.Status404NotFound : StatusCodes.Status400BadRequest,
                detail: errors.First().Description
            )
        );
    }

    [HttpDelete]
    [Route("{orderId}/dishes/{dishId}")]
    public async Task<ActionResult<OrderResponse>> RemoveDishFromOrder(int orderId, int dishId)
    {
        var result = await _orderService.RemoveDishFromOrderAsync(orderId, dishId);
        
        return result.Match(
            order => Ok(new OrderResponse(
                order.Id,
                order.Waiter,
                order.CreatedAt,
                order.OrderDishes.Select(od => new OrderDishResponse(
                    od.Dish.Id,
                    od.Dish.Name,
                    od.Dish.Price
                )).ToList()
            )),
            errors => Problem(
                statusCode: errors.First().Type == ErrorType.NotFound ? StatusCodes.Status404NotFound : StatusCodes.Status400BadRequest,
                detail: errors.First().Description
            )
        );
    }
} 