namespace RestaurantApi.Entities;

public class Dish
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    
    // Navigation property
    public ICollection<OrderDish> OrderDishes { get; set; } = new List<OrderDish>();
} 