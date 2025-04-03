using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using web_api.Dtos;
using web_api.Services;
using web_api.Services.Interfaces;

namespace web_api.Controllers;

[ApiController]
[Route("api/category")]
public class CategoryController : ControllerBase
{
    private readonly ITransactionService _transactionService;
    private readonly ICategoryService _categoryService;

    public CategoryController(ITransactionService transactionService, ICategoryService categoryService)
    {
        _transactionService = transactionService;
        _categoryService = categoryService;
    }

    [HttpPost("add-category")]
    public async Task<IActionResult> AddCategory([FromBody] Dtos.NewCategory transaction)
    {
        await _categoryService.CreateNewCategoryAsync(transaction);

        return Ok();
    }
    
    [HttpGet("get-categories")]
    public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
    {
        return Ok(await _categoryService.GetUserCategoriesAsync());
    }
}
