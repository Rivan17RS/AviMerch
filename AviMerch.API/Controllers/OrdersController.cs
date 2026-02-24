using AviMerch.Application.DTO;
using AviMerch.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Buyer")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateOrderRequest request)
    {
        var buyerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var orderId = await _orderService.CreateOrderAsync(buyerId!, request);

        return Ok(new { OrderId = orderId });
    }
}