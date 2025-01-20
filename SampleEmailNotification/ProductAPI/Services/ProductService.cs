using ProductAPI.Data.Repository;
using ProductAPI.DTO;
using ProductAPI.Models;
using ProductAPI.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<List<ProductDto>> GetAllAsync()
    {
        var products = await _productRepository.GetAllAsync();
        return products.Select(p => new ProductDto(p.Id,
                                                   p.Name,
                                                   p.Description,
                                                   p.Price,
                                                   p.Stock))
                                                   .ToList();
    }

    public async Task<ProductDto> GetByIdAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null) return null;

        return new ProductDto(product.Id,
                              product.Name,
                              product.Description,
                              product.Price,
                              product.Stock);
    }

    public async Task AddAsync(ProductDto productDto)
    {
        var product = new Product
        {
            Id = productDto.Id,
            Name = productDto.Name,
            Description = productDto.Description,
            Price = productDto.Price,
            Stock = productDto.Stock
        };

        await _productRepository.AddAsync(product);
    }

    public async Task UpdateAsync(int id, ProductDto productDto)
    {
        var existingProduct = await _productRepository.GetByIdAsync(id);
        if (existingProduct == null) return;

        existingProduct.Name = productDto.Name;
        existingProduct.Description = productDto.Description;
        existingProduct.Price = productDto.Price;
        existingProduct.Stock = productDto.Stock;

        await _productRepository.UpdateAsync(existingProduct);
    }

    public async Task DeleteAsync(int id)
    {
        await _productRepository.DeleteAsync(id);
    }
}
