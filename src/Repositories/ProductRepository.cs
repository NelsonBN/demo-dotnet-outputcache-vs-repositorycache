using Demo.Entities;

namespace Demo.Repositories;

public interface IProductRepository
{
    IEnumerable<Product> Get();
}

public class ProductRepository : IProductRepository
{
    private readonly ILogger<ProductRepository> _logger;

    public ProductRepository(ILogger<ProductRepository> logger)
        => _logger = logger;

    public IEnumerable<Product> Get()
    {
        _logger.LogInformation($"[REPOSITORY] {DateTime.UtcNow}");

        return Enumerable
            .Range(1, 20)
            .Select(index => new Product
            {
                Id = Guid.NewGuid(),
                Name = $"Product {index}",
                Price = Math.Round(Random.Shared.NextDouble() * 100, 2)
            })
            .ToList();
    }
}
