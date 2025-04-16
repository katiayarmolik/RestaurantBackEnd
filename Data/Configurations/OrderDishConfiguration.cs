using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantApi.Entities;

namespace RestaurantApi.Data.Configurations;

public class OrderDishConfiguration : IEntityTypeConfiguration<OrderDish>
{
    public void Configure(EntityTypeBuilder<OrderDish> builder)
    {
        builder.HasKey(od => new { od.OrderId, od.DishId });
        
        builder.HasOne(od => od.Order)
            .WithMany(o => o.OrderDishes)
            .HasForeignKey(od => od.OrderId);
            
        builder.HasOne(od => od.Dish)
            .WithMany(d => d.OrderDishes)
            .HasForeignKey(od => od.DishId);
    }
} 