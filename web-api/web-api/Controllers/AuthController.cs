using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using web_api.Dtos;
using web_api.Models;
using web_api.Repository;

namespace web_api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;

        public AuthController(IConfiguration configuration, IUserRepository userRepository)
        {
            _configuration = configuration;
            _userRepository = userRepository;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRegisterDto loginDto)
        {
            var user = await _userRepository.GetUserAsync(loginDto.Username);

            if (user != null && Helper.PasswordHelper.ComparePasswords(user.PasswordHash, loginDto.Password))
            {
                var token = GenerateJwtToken(user);
                SetTokenCookie(token);

                return Ok();
            }

            return Unauthorized("Invalid username or password");
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] LoginRegisterDto loginDto)
        {
            // TODO: Add data validation

            if (_userRepository.GetUserAsync(loginDto.Username) != null)
            {
                return Conflict("User with this email already exists");
            }

            var user = await _userRepository.AddUserAsync(new User
            {
                Email = loginDto.Username,
                PasswordHash = Helper.PasswordHelper.GenerateVarbinary64FromPassword(loginDto.Password)
            });

            var token = GenerateJwtToken(user);
            SetTokenCookie(token);

            return Ok();
        }

        [HttpGet("validate-session")]
        public IActionResult ValidateSession()
        {
            if (HttpContext.User.Identity != null && HttpContext.User.Identity.IsAuthenticated)
            {
                return Ok();
            }

            return Unauthorized();
        }


        private void SetTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7),
                //Domain = _configuration["Jwt:Domain"],

                // TODO: Enable for https
                //Secure = true,
                //SameSite = SameSiteMode.Strict
            };
            Response.Cookies.Append("jwt", token, cookieOptions);
        }

        private string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("email", user.Email),
                new Claim("userId", user.UserId.ToString()),
            };

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
