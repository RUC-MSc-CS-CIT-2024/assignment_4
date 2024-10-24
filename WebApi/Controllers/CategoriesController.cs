﻿using DataLayer;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoriesController : ControllerBase
{
    IDataService _dataService;

    public CategoriesController(IDataService dataService)
    {
        _dataService = dataService;
    }

    [HttpGet]
    public IActionResult GetCategories()
    {
        return Ok(_dataService.GetCategories());
    }

    [HttpGet("{id}")]
    public IActionResult GetCategory(int id)
    {
        var category = _dataService.GetCategory(id);

        if(category == null)
        {
            return NotFound();
        }

        return Ok(category);
    }

    [HttpPost]
    public IActionResult CreateCategory(CreateAndUpdateCategoryModel model)
    {
        var category = _dataService.CreateCategory(model.Name, model.Description);
        return Created($"/api/categories/{category.Id}", category);
    }
    
    [HttpPut("{id}")]
    public IActionResult UpdateCategory(int id, CreateAndUpdateCategoryModel model)
    {
        var category = _dataService.GetCategory(id);

        if (category == null)
        {
            return NotFound();
        }

        category.Name = model.Name;
        category.Description = model.Description;
        
        var success = _dataService.UpdateCategory(id, model.Name, model.Description);
    
        if (!success)
        {
            return BadRequest();
        }

        return Ok();
    }
    
    [HttpDelete("{id}")]
    public IActionResult DeleteCategory(int id)
    {
        var result = _dataService.DeleteCategory(id);

        if(result)
        {
            return Ok();
        }
        
        return NotFound();
    }
 
}
