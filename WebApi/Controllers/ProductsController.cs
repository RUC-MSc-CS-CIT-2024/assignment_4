using DataLayer;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    IDataService _dataService;

    public ProductsController(IDataService dataService)
    {
        _dataService = dataService;
    }

    [HttpGet("{id}")]
    public IActionResult GetProducts(int id)
    {
        var product = _dataService.GetProduct(id);

        if (product == null)
        {
            return NotFound();
        }

        return Ok(product);
    }

    [HttpGet("category/{categoryId}")]
    public IActionResult GetProductsByCategory(int categoryId)
    {
        var products = _dataService.GetProductByCategory(categoryId);

        if (products?.Count == 0)
        {
            return NotFound(new List<ProductDTO>());
        }

        foreach (var product in products)
        {
            product.Name = product.Name.Replace("�", "ö");
        }

        return Ok(products);
    }

    [HttpGet]
    public IActionResult GetProductsByName([FromQuery] string name)
    {
        var products = _dataService.GetProductByName(name);

        if (products?.Count == 0)
        {
            return NotFound(new List<ProductSearchModel>());
        }

        foreach (var product in products)
        {
            product.Name = product.Name.Replace("�", "ß");
        }

        return Ok(products);
    }
}