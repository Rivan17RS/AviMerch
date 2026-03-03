using AviMerch.Application.DTO;

public interface IOrderService
{
    Task<Guid> CreateOrderAsync(string buyerId, CreateOrderRequest request);

    Task<List<OrderResponse>> GetOrdersByBuyerAsync(string buyerId);
    
    Task<List<OrderResponse>> GetAllOrdersAsync();
}