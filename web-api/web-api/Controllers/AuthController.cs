using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using web_api.Dtos;
using web_api.Repository;

namespace web_api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepostory;

        public AuthController(IConfiguration configuration, IUserRepository userRepostory)
        {
            _configuration = configuration;
            _userRepostory = userRepostory;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRegisterDto loginDto)
        {
            var user = _userRepostory.GetUser(loginDto.Username);

            if (user != null && Helper.PasswordHelper.ComparePasswords(user.PasswordHash, loginDto.Password))
            {
                var token = GenerateJwtToken(loginDto.Username);
                return Ok(new { Token = token });
            }

            return Unauthorized("Invalid username or password");
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] LoginRegisterDto loginDto)
        {
            // TODO: Add data validation

            if (_userRepostory.GetUser(loginDto.Username) != null)
                return Conflict("User with this email already exists");

            _userRepostory.AddUser(new Models.User
            {
                Email = loginDto.Username,
                PasswordHash = Helper.PasswordHelper.GenerateVarbinary64FromPassword(loginDto.Password)
            });

            var token = GenerateJwtToken(loginDto.Username);

            return Ok(new { Token = token });
        }

        private string GenerateJwtToken(string username)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username)
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
