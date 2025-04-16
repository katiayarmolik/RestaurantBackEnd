using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RestaurantApi.Entities;

namespace RestaurantApi.Data.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.Id);
        
        builder.Property(o => o.Waiter)
            .IsRequired()
            .HasMaxLength(100);
            
        builder.Property(o => o.CreatedAt)
            .IsRequired();
    }
} 