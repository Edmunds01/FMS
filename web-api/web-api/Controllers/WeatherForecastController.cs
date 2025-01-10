using Microsoft.AspNetCore.Mvc;
using web_api.Models;
using web_api.Repository;

namespace web_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly UserRepository _userRepostory;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, UserRepository userRepostory)
        {
            _logger = logger;
            _userRepostory = userRepostory;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            var password = "YourSecurePassword123!";
            var varbinary64 = Helper.PasswordHelper.GenerateVarbinary64FromPassword(password);

            var user = new User
            {
                Email = "example@example.com",
                PasswordHash = varbinary64
            };

            _userRepostory.AddUser(user);

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
