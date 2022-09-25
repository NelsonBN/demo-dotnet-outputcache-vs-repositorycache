# Outputcache vs Repository Cache

New .NET feature `OutputCache`

---
## Without Cache

### `Program.cs`
```csharp
...
builder.Services.AddScoped<IProductRepository, ProductRepository>();
...
```

### `Controller.cs`
```csharp
[ApiController]
[Route("[controller]")]
public class Controller : ControllerBase
{
    private readonly IProductRepository _repository;

    public WithoutCacheController(IProductRepository repository)
     => _repository = repository;

    [HttpGet]
    public IActionResult Get()
        => Ok(_repository.Get());
}
```

### `Repository.cs`
```csharp
public class ProductRepository : IProductRepository
{
    public IEnumerable<Product> Get()
        => Enumerable
            .Range(1, 20)
            .Select(index => new Product
            {
                Id = Guid.NewGuid(),
                Name = $"Product {index}",
                Price = Math.Round(Random.Shared.NextDouble() * 100, 2)
            })
            .ToList();
}
```

---
## With MemoryCache

### `Program.cs`
```csharp
...
builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddMemoryCache();
builder.Services.AddScoped<IProductCache, ProductCache>();
...
```

### `Controller.cs`
```csharp
[ApiController]
[Route("[controller]")]
public class Controller : ControllerBase
{
    private readonly IProductCache _cache;

    public WithoutCacheController(IProductCache cache)
     => _cache = cache;

    [HttpGet]
    public IActionResult Get()
        => Ok(_cache.Get());
}
```

### `Cache.cs`
Maybe here you will use the design pattern as a **Decorator**
```csharp
public class ProductCache : IProductCache
{
    private readonly IMemoryCache _cache;
    private readonly IProductRepository _repository;

    public ProductCache(
        IMemoryCache cache,
        IProductRepository repository
    )
    {
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

        return products;
    }
}
```

### `Repository.cs`
```csharp
public class ProductRepository : IProductRepository
{
    public IEnumerable<Product> Get()
        => Enumerable
            .Range(1, 20)
            .Select(index => new Product
            {
                Id = Guid.NewGuid(),
                Name = $"Product {index}",
                Price = Math.Round(Random.Shared.NextDouble() * 100, 2)
            })
            .ToList();
}
```



## With OutputCache

### `Program.cs`
```csharp
...
builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddOutputCache(options =>
{
    options.DefaultExpirationTimeSpan = TimeSpan.FromSeconds(5);
});
...
app.UseOutputCache();
...
```

### `Controller.cs`
```csharp
[ApiController]
[Route("[controller]")]
public class Controller : ControllerBase
{
    private readonly IProductRepository _repository;

    public WithoutCacheController(IProductRepository repository)
     => _repository = repository;

    [HttpGet]
    public IActionResult Get()
        => Ok(_repository.Get());
}
```

### `Repository.cs`
```csharp
public class ProductRepository : IProductRepository
{
    public IEnumerable<Product> Get()
        => Enumerable
            .Range(1, 20)
            .Select(index => new Product
            {
                Id = Guid.NewGuid(),
                Name = $"Product {index}",
                Price = Math.Round(Random.Shared.NextDouble() * 100, 2)
            })
            .ToList();
}
```