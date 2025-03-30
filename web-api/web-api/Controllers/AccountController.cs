using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using web_api.Dtos;
using web_api.Services;

namespace web_api.Controllers;

[ApiController]
[Route("api/account")]
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
        return Ok(_accountService.GetUserAccounts(1));
    }
}
