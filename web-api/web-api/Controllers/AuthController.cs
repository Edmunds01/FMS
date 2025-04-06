using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using web_api.Dtos;
using web_api.Helper.Interfaces;
using web_api.Models;
using web_api.Repository.Interfaces;

namespace web_api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IConfiguration configuration, IUserRepository userRepository, ITokenHelper tokenHelper) : ControllerBase
{
    private readonly IConfiguration _configuration = configuration;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ITokenHelper _tokenHelper = tokenHelper;

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRegisterDto loginDto)
    {
        var user = await _userRepository.GetUserAsync(loginDto.Username);

        if (user != null && Helper.PasswordHelper.ComparePasswords(user.PasswordHash, loginDto.Password))
        {
            SetTokenCookie(user);

            return Ok();
        }

        return Unauthorized(JsonSerializer.Serialize("Invalid username or password"));
    }

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register([FromBody] LoginRegisterDto loginDto)
    {
        // TODO: Add data validation

        if (await _userRepository.GetUserAsync(loginDto.Username) != null)
        {
            return Conflict("User with this email already exists");
        }

        var user = await _userRepository.AddUserAsync(new User
        {
            Email = loginDto.Username,
            PasswordHash = Helper.PasswordHelper.GenerateVarbinary64FromPassword(loginDto.Password)
        });

        SetTokenCookie(user);

        return Ok();
    }

    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Logout()
    {
        Response.Cookies.Delete(_tokenHelper.JwtCookieName);

        return Ok();
    }

    [HttpGet("validate-session")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<bool> ValidateSession() =>
        HttpContext.User.Identity != null && HttpContext.User.Identity.IsAuthenticated ? Ok(true) : Ok(false);

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
