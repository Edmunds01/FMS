using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using web_api.Dtos;
using web_api.Services;
using web_api.Services.Interfaces;

namespace web_api.Controllers;

[ApiController]
[Route("api/transaction")]
public class TransactionController(ITransactionService transactionService) : ControllerBase
{
    private readonly ITransactionService _transactionService = transactionService;

    [HttpGet("category-transactions")]
    public ActionResult<IEnumerable<Transaction>> GetTransaction(long categoryId)
    {
        return Ok(_transactionService.GetUserTransactions(categoryId));
    }
    
    [HttpPost("add-transaction")]
    public async Task<IActionResult> AddTransaction([FromBody] Dtos.Transaction transaction)
    {
        await _transactionService.CreateNewTransactionAsync(transaction);

        return Ok();
    }

    [HttpPost("save-transaction-account")]
    public Task<IActionResult> SaveTransactionAccount(long transactionId, long newAccountId)
    {
        throw new NotImplementedException();
    }

    [HttpPost("save-transaction-category")]
    public Task<IActionResult> SaveTransactionCategory(long transactionId, long newCategoryId)
    {
        throw new NotImplementedException();
    }

    [HttpPost("save-transaction-amount")]
    public Task<IActionResult> SaveTransactionAmount(long transactionId, decimal newAmount)
    {
        throw new NotImplementedException();
    }

    [HttpPost("save-transaction-comment")]
    public Task<IActionResult> SaveTransactionComment(long transactionId, string newComment)
    {
        throw new NotImplementedException();
    }

    [HttpPost("save-transaction-date")]
    public Task<IActionResult> SaveTransactionDate(long transactionId, DateTime newDate)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("delete-transaction")]
    public Task<IActionResult> DeleteTransaction(long transactionId)
    {
        throw new NotImplementedException();
    }
}
