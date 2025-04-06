namespace web_api.Helper.Interfaces;

public interface ITokenHelper
{
    string JwtCookieName { get; }
    string GenerateJwtToken(Models.User user);
    bool ValidateToken(HttpContext context);
}
