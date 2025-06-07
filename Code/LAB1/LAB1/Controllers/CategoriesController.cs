using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using LAB1.DTOs;
using LAB1.Services;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    [EnableQuery]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> Get()
    {
        var categories = await _categoryService.GetAllCategoriesAsync();
        return Ok(categories);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryDto>> Get(int id)
    {
        try
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            return Ok(category);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost]
    public async Task<ActionResult<CategoryDto>> Post([FromBody] CategoryDto categoryDto)
    {
        var createdCategory = await _categoryService.CreateCategoryAsync(categoryDto);
        return CreatedAtAction(nameof(Get), new { id = createdCategory.Id }, createdCategory);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CategoryDto>> Put(int id, [FromBody] CategoryDto categoryDto)
    {
        try
        {
            var updatedCategory = await _categoryService.UpdateCategoryAsync(id, categoryDto);
            return Ok(updatedCategory);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            await _categoryService.DeleteCategoryAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}