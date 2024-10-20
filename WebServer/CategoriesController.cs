using DataLayer;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/categories")]
public class CategoriesController : ControllerBase
{
    private readonly IDataService _dataService;

    public CategoriesController(IDataService dataService)
    {
        _dataService = dataService;
    }

    [HttpGet()]
    public IActionResult Get()
        => Ok(_dataService.GetCategories());

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        Category? c = _dataService.GetCategory(id);
        return c == null 
            ? NotFound() 
            : Ok(c);
    }

    [HttpPost()]
    public IActionResult Post([FromBody] CategoryRequest category)
    {
        Category c = _dataService.CreateCategory(category.Name, category.Description);
        return CreatedAtAction(nameof(Get), c);
    }

    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody] CategoryRequest category)
        => _dataService.UpdateCategory(id, category.Name, category.Description)
            ? Ok()
            : NotFound();

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
        => _dataService.DeleteCategory(id)
            ? Ok()
            : NotFound();
}