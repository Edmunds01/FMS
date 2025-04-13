using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using web_api.Attributes;
using web_api.Services.Interfaces;

namespace web_api.Controllers;

[ApiController]
[Route("api/user")]
[NotAuthorizedExceptionFilter]
[GlobalExceptionFilter]
public class UserController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;

    [HttpGet("user-email")]
    [AuditLog("Get user email action")]
    public async Task<ActionResult<string>> GetEmail() => Ok(JsonSerializer.Serialize(await _userService.GetUserEmailAsync()));

    [HttpPost("change-password")]
    [AuditLog("Change user password action")]
    public async Task<IActionResult> ChangePassword(string oldPassword, string newPassword)
    {
        await _userService.ChangeUserPasswordAsync(oldPassword, newPassword);
        return Ok();
    }
}
