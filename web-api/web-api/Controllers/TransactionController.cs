using Microsoft.AspNetCore.Mvc;
using web_api.Attributes;
using web_api.Dtos;
using web_api.Services.Interfaces;

namespace web_api.Controllers;

[ApiController]
[Route("api/transaction")]
[NotAuthorizedExceptionFilter]
[GlobalExceptionFilter]
public class TransactionController(ITransactionService transactionService) : ControllerBase
{
    private readonly ITransactionService _transactionService = transactionService;

    [HttpGet("category-transactions")]
    [AuditLog("Get transaction action")]
    public ActionResult<IEnumerable<Transaction>> GetTransactions(long categoryId, DateTime startDate, DateTime endDate) => Ok(_transactionService.GetUserTransactions(categoryId, startDate, endDate));

    [HttpPost("upsert-transaction")]
    [AuditLog("Upsert transaction action")]
    public async Task<IActionResult> UpsertTransaction([FromBody] Transaction transaction)
    {
        await _transactionService.UpsertTransactionAsync(transaction);

        return Ok();
    }

    [HttpDelete("delete-transaction")]
    [AuditLog("Delete transaction action")]
    public async Task<IActionResult> DeleteTransaction(long transactionId)
    {
        await _transactionService.DeleteTransactionAsync(transactionId);

        return Ok();
    }
}
