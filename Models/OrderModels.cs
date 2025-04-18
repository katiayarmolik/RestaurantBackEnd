using System.ComponentModel.DataAnnotations;

namespace RestaurantApi.Models;

public record CreateOrderRequest(
    [Required]
    [StringLength(100)]
    string Waiter
);

public record UpdateOrderRequest(
    [Required]
    [StringLength(100)]
    string Waiter
);

public record OrderDishResponse(
    int Id,
    string Name,
    decimal Price
);

public record OrderResponse(
    int Id,
    string Waiter,
    DateTime CreatedAt,
    List<OrderDishResponse> Dishes
); 