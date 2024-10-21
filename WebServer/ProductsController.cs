using DataLayer;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/products")]
public class ProductsController: ControllerBase
{
    private readonly IDataService _dataService;

    public ProductsController(IDataService dataService)
    {
        _dataService = dataService;
    }

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        Product? p = _dataService.GetProduct(id);
        return p == null ? NotFound() : Ok(p);
    }

    [HttpGet("category/{id}")]
    public IActionResult GetByCategory(int id) {
        List<Product> products = _dataService.GetProductByCategory(id);
        return products.Count == 0 ? NotFound(Enumerable.Empty<object>()) : Ok(products);
    }

    [HttpGet()]
    public IActionResult GetWithFilter([FromQuery] string name)
    {
        List<SimpleProductDto> products = _dataService.GetProductByName(name);
        return products.Count == 0 ? NotFound(Enumerable.Empty<object>()) : Ok(products);
    }
}