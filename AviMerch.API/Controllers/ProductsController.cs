using AviMerch.Application.DTOs;
using AviMerch.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

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

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var product = await _productService.GetProductByIdAsync(id);

        if (product == null)
            return NotFound();

        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateProductDto dto)
    {
        var product = await _productService.CreateProductAsync(dto);

        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateProductDto dto)
    {
        var success = await _productService.UpdateProductAsync(id, dto);

        if (!success)
            return NotFound();

        return NoContent(); // 204 REST compliant
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var success = await _productService.DeleteProductAsync(id);

        if (!success)
            return NotFound();

        return NoContent(); // 204 REST compliant
    }
}
