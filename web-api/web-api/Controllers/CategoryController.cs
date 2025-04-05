using Microsoft.AspNetCore.Mvc;
using web_api.Dtos;
using web_api.Services.Interfaces;

namespace web_api.Controllers;

[ApiController]
[Route("api/category")]
public class CategoryController(ICategoryService categoryService) : ControllerBase
{
    private readonly ICategoryService _categoryService = categoryService;

    [HttpPost("add-category")]
    public async Task<IActionResult> AddCategory([FromBody] Dtos.NewCategory transaction)
    {
        await _categoryService.CreateNewCategoryAsync(transaction);

        return Ok();
    }

    [HttpGet("get-categories")]
    public ActionResult<IEnumerable<Category>> GetCategories()
    {
        return Ok(_categoryService.GetUserCategories());
    }

    [HttpPost("save-category-icon")]
    public async Task<IActionResult> SaveCategoryIcon(long categoryId, string icon)
    { 
        await _categoryService.SaveCategoryIconAsync(categoryId, icon);

        return Ok();
    }
    
    [HttpPost("save-category-name")]
    public async Task<IActionResult> SaveCategoryName(long categoryId, string name)
    { 
        await _categoryService.SaveCategoryNameAsync(categoryId, name);

        return Ok();
    }
    
    [HttpDelete("delete-category")]
    public async Task<IActionResult> DeleteCategory(long categoryId)
    { 
        await _categoryService.DeleteCategoryAsync(categoryId);

        return Ok();
    }
}
