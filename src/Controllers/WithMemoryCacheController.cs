using Demo.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace src.Controllers;

[ApiController]
[Route("[controller]")]
public class WithMemoryCacheController : ControllerBase
{
    private readonly ILogger<WithMemoryCacheController> _logger;
    private readonly IProductCache _cache;

    public WithMemoryCacheController(
        ILogger<WithMemoryCacheController> logger,
        IProductCache cache
    )
    {
        _logger = logger;
        _cache = cache;
    }

    [HttpGet]
    public IActionResult Get()
    {
        _logger.LogInformation($"[CONTROLLER] {DateTime.UtcNow}");
        return Ok(_cache.Get());
    }
}
