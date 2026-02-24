using AviMerch.Domain.Enums;

namespace AviMerch.Domain.Entities;

public class Order
{
    public Guid Id { get; set; }

    // Identity string ID
    public string BuyerId { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public OrderStatus Status { get; set; }

    public decimal TotalAmount { get; set; }

    // Navigation property
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}