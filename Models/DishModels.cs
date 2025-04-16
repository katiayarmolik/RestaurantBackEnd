using System.ComponentModel.DataAnnotations;

namespace RestaurantApi.Models;

public record CreateDishRequest(
    [Required]
    [StringLength(100)]
    string Name,
    
    [Required]
    [Range(0.01, double.MaxValue)]
    decimal Price
);

public record UpdateDishRequest(
    [Required]
    [StringLength(100)]
    string Name,
    
    [Required]
    [Range(0.01, double.MaxValue)]
    decimal Price
);

public record DishResponse(
    int Id,
    string Name,
    decimal Price
); 