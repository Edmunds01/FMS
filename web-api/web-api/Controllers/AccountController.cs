using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using web_api.Dtos;
using web_api.Services;

namespace web_api.Controllers;

[ApiController]
[Route("api/account")]
[Authorize]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpGet("accounts")]
    public ActionResult<IEnumerable<Account>> GetAccounts()
    {
        return Ok(_accountService.GetUserAccounts());
    }

    [HttpPost("save-account-icon")]
    public async Task<IActionResult> SaveAccountIcon(long accountId, string accountIcon)
    {
        if (await _accountService.SaveAccountIconAsync(accountId, accountIcon))
        {
            return Ok();
        }

        return NotFound("Account not found");
    }

    [HttpPost("save-account-name")]
    public async Task<IActionResult> SaveAccountName(long accountId, string accountName)
    {
        if (await _accountService.SaveAccountNameAsync(accountId, accountName))
        {
            return Ok();
        }

        return NotFound("Account not found");
    }

    [HttpPost("create-new-account")]
    public async Task<ActionResult> CreateNewAccount([FromBody] Account account)
    {
        await _accountService.CreateNewAccountAsync(account);

        return Ok();
    }

    [HttpDelete("delete-account")]
    public async Task<ActionResult> DeleteAccount(long accountId)
    {
        if (await _accountService.DeleteAccountAsync(accountId))
        {
            return Ok();
        }

        return NotFound("Account not found");
    }
}
