using Demo.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace src.Controllers;

[ApiController]
[Route("[controller]")]
public class WithOutputCacheController : ControllerBase
{
    private readonly ILogger<WithoutCacheController> _logger;
    private readonly IProductRepository _repository;

    public WithOutputCacheController(
        ILogger<WithoutCacheController> logger,
        IProductRepository repository
    )
    {
        _logger = logger;
        _repository = repository;
    }

    [HttpGet]
    [OutputCache]
    public IActionResult Get()
    {
        _logger.LogInformation($"[CONTROLLER] {DateTime.UtcNow}");
        return Ok(_repository.Get());
    }
}
