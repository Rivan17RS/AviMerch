using AviMerch.Application.DTO;
using AviMerch.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace AviMerch.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    // PUBLIC - Browse products
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetProducts(int pageNumber = 1, int pageSize = 10)
    {
        var products = await _productService.GetProductsAsync(pageNumber, pageSize);
        var totalCount = await _productService.GetTotalCountAsync();

        Response.Headers["X-Total-Count"] = totalCount.ToString();

        return Ok(new
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount,
            Data = products
        });
    }

    // PUBLIC - Get single product
    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var product = await _productService.GetProductByIdAsync(id);

        if (product == null)
            return NotFound();

        return Ok(product);
    }

    // Seller/Admin - Create product
    [Authorize(Roles = "Seller,Admin")]
    [HttpPost]
    public async Task<IActionResult> CreateProduct(CreateProductDto dto)
    {

        var createdProduct = await _productService.CreateProductAsync(dto);

        return CreatedAtAction(
            nameof(GetById),
            new { id = createdProduct.Id },
            createdProduct
        );
    }

    // Seller (owner) or Admin - Update product
    [Authorize(Roles = "Seller,Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateProductDto dto)
    {
        var userId = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        var isAdmin = User.IsInRole("Admin");

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var product = await _productService.GetProductByIdAsync(id);

        if (product == null)
            return NotFound();

        // Ownership check
        if (product.SellerId.ToString() != userId && !isAdmin)
            return Forbid();

        var success = await _productService.UpdateProductAsync(id, dto);

        if (!success)
            return NotFound();

        return NoContent(); // 204 REST compliant
    }

    // Admin only - Delete product
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var success = await _productService.DeleteProductAsync(id);

        if (!success)
            return NotFound();

        return NoContent(); // 204 REST compliant
    }

    // Seller/Admin - Get only my products
    [Authorize(Roles = "Seller,Admin")]
    [HttpGet("my-products")]
    public async Task<IActionResult> GetMyProducts()
    {
        var userId = User.FindFirstValue(JwtRegisteredClaimNames.Sub);

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var products = await _productService.GetProductsBySellerAsync(userId);

        return Ok(products);
    }
}