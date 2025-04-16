namespace RestaurantApi.Entities;

public class Order
{
    public int Id { get; set; }
    public string Waiter { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation property
    public ICollection<OrderDish> OrderDishes { get; set; } = new List<OrderDish>();
} 