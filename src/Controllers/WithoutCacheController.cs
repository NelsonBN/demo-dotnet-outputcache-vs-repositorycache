using Demo.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace src.Controllers;

[ApiController]
[Route("[controller]")]
public class WithoutCacheController : ControllerBase
{
    private readonly ILogger<WithoutCacheController> _logger;
    private readonly IProductRepository _repository;

    public WithoutCacheController(
        ILogger<WithoutCacheController> logger,
        IProductRepository repository
    )
    {
        _logger = logger;
        _repository = repository;
    }

    [HttpGet]
    public IActionResult Get()
    {
        _logger.LogInformation($"[CONTROLLER] {DateTime.UtcNow}");
        return Ok(_repository.Get());
    }
}
