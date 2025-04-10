using Microsoft.AspNetCore.Mvc;
using web_api.Attributes;
using web_api.Dtos;
using web_api.Services.Interfaces;

namespace web_api.Controllers;

[ApiController]
[Route("api/category")]
public class CategoryController(ICategoryService categoryService) : ControllerBase
{
    private readonly ICategoryService _categoryService = categoryService;

    [HttpPost("add-category")]
    [AuditLog("Add category action")]
    public async Task<IActionResult> AddCategory([FromBody] Dtos.NewCategory transaction)
    {
        await _categoryService.CreateNewCategoryAsync(transaction);

        return Ok();
    }

    [HttpGet("categories")]
    [AuditLog("Get categories action")]
    public ActionResult<IEnumerable<Category>> GetCategories(DateTime startDate, DateTime endDate) => Ok(_categoryService.GetUserCategories(startDate, endDate));

    [HttpPost("save-category-icon")]
    [AuditLog("Save category icon action")]
    public async Task<IActionResult> SaveCategoryIcon(long categoryId, string icon)
    {
        await _categoryService.SaveCategoryIconAsync(categoryId, icon);

        return Ok();
    }

    [HttpPost("save-category-name")]
    [AuditLog("Save category name action")]
    public async Task<IActionResult> SaveCategoryName(long categoryId, string name)
    {
        await _categoryService.SaveCategoryNameAsync(categoryId, name);

        return Ok();
    }

    [HttpDelete("delete-category")]
    [AuditLog("Delete category action")]
    public async Task<IActionResult> DeleteCategory(long categoryId)
    {
        await _categoryService.DeleteCategoryAsync(categoryId);

        return Ok();
    }
}
