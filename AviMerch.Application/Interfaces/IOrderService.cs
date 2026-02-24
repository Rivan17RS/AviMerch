using AviMerch.Application.DTO;

public interface IOrderService
{
    Task<Guid> CreateOrderAsync(string buyerId, CreateOrderRequest request);
}