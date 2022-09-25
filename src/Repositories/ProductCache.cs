using Demo.Entities;
using Microsoft.Extensions.Caching.Memory;

namespace Demo.Repositories;

public interface IProductCache : IProductRepository { }
public class ProductCache : IProductCache
{
    private readonly ILogger<ProductCache> _logger;
    private readonly IMemoryCache _cache;
    private readonly IProductRepository _repository;

    public ProductCache(
        ILogger<ProductCache> logger,
        IMemoryCache cache,
        IProductRepository repository
    )
    {
        _logger = logger;
        _cache = cache;
        _repository = repository;
    }

    public IEnumerable<Product> Get()
    {
        const string KEY = "PRODUCTS";

        var products = _cache.Get<IEnumerable<Product>>(KEY);
        if(products is null)
        {
            products = _repository.Get();
            _cache.Set(KEY, products, TimeSpan.FromSeconds(5));
        }

        _logger.LogInformation($"[CACHE] {DateTime.UtcNow}");

        return products;
    }
}
