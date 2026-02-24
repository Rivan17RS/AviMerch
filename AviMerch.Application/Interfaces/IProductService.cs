using AviMerch.Application.DTO;
using AviMerch.Domain.Entities;

namespace AviMerch.Application.Interfaces;

public interface IProductService
{
    Task<IEnumerable<Product>> GetProductsAsync(int pageNumber, int pageSize);
    Task<IEnumerable<Product>> GetProductsBySellerAsync(string sellerId);  
    Task<Product?> GetProductByIdAsync(Guid id);
    Task<Product> CreateProductAsync(CreateProductDto dto);
    Task<bool> UpdateProductAsync(Guid id, UpdateProductDto dto);
    Task<bool> DeleteProductAsync(Guid id);
    Task<int> GetTotalCountAsync();

}
