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
        public ActionResult<TokenResponse> Login([FromBody] LoginRegisterDto loginDto)
        {
            var user = _userRepository.GetUser(loginDto.Username);

            if (user != null && Helper.PasswordHelper.ComparePasswords(user.PasswordHash, loginDto.Password))
            {
                return Ok(new TokenResponse() { Token = GenerateJwtToken(user) });
            }

            return Unauthorized("Invalid username or password");
        }

        [HttpPost("register")]
        public async Task<ActionResult<TokenResponse>> Register([FromBody] LoginRegisterDto loginDto)
        {
            // TODO: Add data validation

            if (_userRepository.GetUser(loginDto.Username) != null)
            {
                return Conflict("User with this email already exists");
            }

            var user = await _userRepository.AddUserAsync(new User
            {
                Email = loginDto.Username,
                PasswordHash = Helper.PasswordHelper.GenerateVarbinary64FromPassword(loginDto.Password)
            });

            return Ok(new TokenResponse() { Token = GenerateJwtToken(user) });
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
