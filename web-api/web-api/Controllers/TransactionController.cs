using Microsoft.AspNetCore.Mvc;
using web_api.Dtos;
using web_api.Services.Interfaces;

namespace web_api.Controllers;

[ApiController]
[Route("api/transaction")]
public class TransactionController(ITransactionService transactionService) : ControllerBase
{
    private readonly ITransactionService _transactionService = transactionService;

    [HttpGet("category-transactions")]
    public ActionResult<IEnumerable<Transaction>> GetTransaction(long categoryId) => Ok(_transactionService.GetUserTransactions(categoryId));

    [HttpPost("upsert-transaction")]
    public async Task<IActionResult> UpsertTransaction([FromBody] Transaction transaction)
    {
        await _transactionService.UpsertTransactionAsync(transaction);

        return Ok();
    }

    [HttpDelete("delete-transaction")]
    public async Task<IActionResult> DeleteTransaction(long transactionId)
    {
        await _transactionService.DeleteTransactionAsync(transactionId);

        return Ok();
    }
}
