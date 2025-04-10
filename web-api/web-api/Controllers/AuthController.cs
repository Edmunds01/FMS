using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using web_api.Attributes;
using web_api.Dtos;
using web_api.Helper;
using web_api.Helper.Interfaces;
using web_api.Models;
using web_api.Repository.Interfaces;

namespace web_api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController
    (
        IConfiguration configuration,
        IUserRepository userRepository,
        ITokenHelper tokenHelper,
        IRecoverHelper recoverHelper,
        ICategoryRepository categoryRepository
    ) : ControllerBase
{
    private readonly IConfiguration _configuration = configuration;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ICategoryRepository _categoryRepository = categoryRepository;
    private readonly ITokenHelper _tokenHelper = tokenHelper;
    private readonly IRecoverHelper _recoverHelper = recoverHelper;

    [HttpPost("login")]
    [AuditLog("Login action")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Login([FromBody] LoginRegisterDto loginDto)
    {
        var user = await _userRepository.GetUserAsync(loginDto.Username);

        if (user != null && Helper.PasswordHelper.ComparePasswords(user.PasswordHash, loginDto.Password))
        {
            SetTokenCookie(user);

            return Ok();
        }

        return BadRequest(JsonSerializer.Serialize("Invalid username or password"));
    }

    [HttpPost("register")]
    [AuditLog("Register action")]
    public async Task<IActionResult> Register([FromBody] LoginRegisterDto loginDto)
    {
        if (await _userRepository.GetUserAsync(loginDto.Username) != null)
        {
            return Conflict("User with this email already exists");
        }

        var user = await _userRepository.AddUserAsync(new User
        {
            Email = loginDto.Username,
            PasswordHash = PasswordHelper.GenerateVarbinary64FromPassword(loginDto.Password)
        });

        SetTokenCookie(user);

        await SetDefaultUserValues();

        return Ok();

        async Task SetDefaultUserValues()
        {
            await _categoryRepository.InsertAsync(new Models.Category
            {
                UserId = user.UserId,
                Name = "Pārtika",
                Icon = "pizza-slice",
                Type = 2,
            });
            
            await _categoryRepository.InsertAsync(new Models.Category
            {
                UserId = user.UserId,
                Name = "Īre",
                Icon = "house",
                Type = 2,
            });
            
            await _categoryRepository.InsertAsync(new Models.Category
            {
                UserId = user.UserId,
                Name = "Alga",
                Icon = "sack-dollar",
                Type = 1,
            });
        }
    }

    [AuditLog("Logout action")]
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete(_tokenHelper.JwtCookieName);

        return Ok();
    }

    [HttpGet("validate-session")]
    [AuditLog("Validate session action")]
    public ActionResult<bool> ValidateSession() =>
        HttpContext.User.Identity != null && HttpContext.User.Identity.IsAuthenticated ? Ok(true) : Ok(false);

    [HttpPost("recover")]
    [AuditLog("Recover action")]
    public async Task<IActionResult> Recover([EmailAddress] string email)
    {
        var user = await _userRepository.GetUserAsync(email);
        if (user == null)
        {
            return BadRequest("User with this email does not exist");
        }

        await _recoverHelper.SendRecoveryEmailAsync(email);

        return Ok();
    }

    [HttpPost("recover-change-password")]
    [AuditLog("Recover change password")]
    public async Task<IActionResult> RecoverChangePassword(Guid token, string password)
    {
        var isValid = _recoverHelper.ValidateRecoveryToken(token, out var email);

        if (!isValid)
        {
            return BadRequest("Invalid token");
        }

        var user = await _userRepository.GetUserAsync(email!) ?? throw new NotSupportedException();

        user.PasswordHash = PasswordHelper.GenerateVarbinary64FromPassword(password);
        await _userRepository.SaveChangesAsync();

        return Ok();
    }

    private void SetTokenCookie(User user)
    {
        var token = _tokenHelper.GenerateJwtToken(user);

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddDays(7),
        };

        if (bool.TryParse(_configuration["IS_DEPLOYMENT"], out var isDeployment) && isDeployment)
        {
            cookieOptions.Secure = true;
            cookieOptions.SameSite = SameSiteMode.None;
        }

        Response.Cookies.Append(_tokenHelper.JwtCookieName, token, cookieOptions);
    }
}
