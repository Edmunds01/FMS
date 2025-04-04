using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace web_api.Middleware;

public class ConditionalAuthorizeMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;

    public ConditionalAuthorizeMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _configuration = configuration;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (IsAuthController(context) || IsSwaggerRequest(context))
        {
            await _next(context);
            return;
        }

        if (!ValidateTokenFromCookie(context))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        if (!IsAuthenticated(context))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        await _next(context);
    }

    private static bool IsAuthController(HttpContext context)
    {
        var endpoint = context.GetEndpoint();
        if (endpoint != null)
        {
            var metadata = endpoint.Metadata.GetMetadata<Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor>();
            var controllerName = metadata?.ControllerName;
            var actionName = metadata?.ActionName;
            return controllerName == "Auth" && actionName != "ValidateSession";
        }

        return false;
    }

    private static bool IsSwaggerRequest(HttpContext context)
    {
        return context.Request.Path.StartsWithSegments("/swagger");
    }

    private bool ValidateTokenFromCookie(HttpContext context)
    {
        var token = context.Request.Cookies["jwt"];
        if (string.IsNullOrEmpty(token))
        {
            return false;
        }

        try
        {
            // TODO: Move to a helper or service
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!);
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidAudience = _configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(key)
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var claims = new List<Claim>(jwtToken.Claims);

            var identity = new ClaimsIdentity(claims, "jwt");
            context.User = new ClaimsPrincipal(identity);
            return true;
        }
        catch
        {
            return false;
        }
    }

    private bool IsAuthenticated(HttpContext context)
    {
        var isAuthenticated = context.User.Identity?.IsAuthenticated;
        var notRequiredAuth = _configuration.GetValue<bool>("NOT_REQUIRED_AUTH");
        return notRequiredAuth || (isAuthenticated.HasValue && isAuthenticated.Value);
    }
}
