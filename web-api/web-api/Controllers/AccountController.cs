using Microsoft.AspNetCore.Mvc;
using web_api.Attributes;
using web_api.Dtos;
using web_api.Services.Interfaces;

namespace web_api.Controllers;

[ApiController]
[Route("api/account")]
[NotAuthorizedExceptionFilter]
[GlobalExceptionFilter]
public class AccountController(IAccountService accountService) : ControllerBase
{
    private readonly IAccountService _accountService = accountService;

    [HttpGet("accounts")]
    [AuditLog("Get accounts action")]
    public ActionResult<IEnumerable<Account>> GetAccounts() => Ok(_accountService.GetUserAccounts());

    [HttpPost("save-account-icon")]
    [AuditLog("Save account icon action")]
    public async Task<IActionResult> SaveAccountIcon(long accountId, string accountIcon)
    {
        await _accountService.SaveAccountIconAsync(accountId, accountIcon);

        return Ok();
    }

    [HttpPost("save-account-name")]
    [AuditLog("Save account name action")]
    public async Task<IActionResult> SaveAccountName(long accountId, string accountName)
    {
        await _accountService.SaveAccountNameAsync(accountId, accountName);

        return Ok();
    }

    [HttpPost("add-account")]
    [AuditLog("Add account action")]
    public async Task<IActionResult> AddAccount([FromBody] NewAccount account)
    {
        await _accountService.CreateNewAccountAsync(account);

        return Ok();
    }

    [HttpDelete("delete-account")]
    [AuditLog("Delete account action")]
    public async Task<ActionResult> DeleteAccount(long accountId)
    {
        await _accountService.DeleteAccountAsync(accountId);

        return Ok();
    }
}
