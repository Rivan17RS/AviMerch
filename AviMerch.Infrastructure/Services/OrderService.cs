using AviMerch.Application.DTO;
using AviMerch.Application.Interfaces;
using AviMerch.Domain.Entities;
using AviMerch.Domain.Enums;
using AviMerch.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

public class OrderService : IOrderService
{
    private readonly AppDbContext _context;

    public OrderService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> CreateOrderAsync(string buyerId, CreateOrderRequest request)
    {
        if (request.Items == null || !request.Items.Any())
            throw new ArgumentException("Order must contain at least one item.");

        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var productIds = request.Items.Select(i => i.ProductId).ToList();

            var products = await _context.Products
                .Where(p => productIds.Contains(p.Id))
                .ToListAsync();

            // HARD FAIL if any product missing
            if (products.Count != productIds.Count)
                throw new InvalidOperationException("One or more products do not exist.");

            var order = new Order
            {
                Id = Guid.NewGuid(),
                BuyerId = buyerId,
                CreatedAt = DateTime.UtcNow,
                Status = OrderStatus.Pending
            };

            decimal totalAmount = 0m;

            foreach (var item in request.Items)
            {
                var product = products.First(p => p.Id == item.ProductId);

                if (item.Quantity <= 0)
                    throw new InvalidOperationException("Invalid quantity.");

                if (product.StockQuantity < item.Quantity)
                    throw new InvalidOperationException(
                        $"Insufficient stock for product {product.Name}");

                product.StockQuantity -= item.Quantity;

                var lineTotal = product.Price * item.Quantity;
                totalAmount += lineTotal;

                order.OrderItems.Add(new OrderItem
                {
                    Id = Guid.NewGuid(),
                    ProductId = product.Id,
                    Quantity = item.Quantity,
                    UnitPrice = product.Price
                });
            }

            order.TotalAmount = totalAmount;

            _context.Orders.Add(order);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return order.Id;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<List<OrderResponse>> GetOrdersByBuyerAsync(string buyerId)
    {
        var orders = await _context.Orders
            .Include(o => o.OrderItems)
            .Where(o => o.BuyerId == buyerId)
            .ToListAsync();

        return orders.Select(o => new OrderResponse
        {
            OrderId = o.Id,
            CreatedAt = o.CreatedAt,
            TotalAmount = o.TotalAmount,
            Status = o.Status.ToString(),
            Items = o.OrderItems.Select(i => new OrderItemResponse
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice
            }).ToList()
        }).ToList();
    }

    public async Task<List<OrderResponse>> GetAllOrdersAsync()
    {
        var orders = await _context.Orders
            .Include(o => o.OrderItems)
            .ToListAsync();

        return orders.Select(o => new OrderResponse
        {
            OrderId = o.Id,
            CreatedAt = o.CreatedAt,
            TotalAmount = o.TotalAmount,
            Status = o.Status.ToString(),
            Items = o.OrderItems.Select(i => new OrderItemResponse
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice
            }).ToList()
        }).ToList();
    }
}