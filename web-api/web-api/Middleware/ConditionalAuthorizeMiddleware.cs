using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using web_api.Helper.Interfaces;

namespace web_api.Middleware;

public class ConditionalAuthorizeMiddleware(RequestDelegate next, ITokenHelper tokenHelper)
{
    private readonly RequestDelegate _next = next;
    private readonly ITokenHelper _tokenHelper = tokenHelper;

    public async Task InvokeAsync(HttpContext context)
    {
        if (!_tokenHelper.ValidateToken(context))
        {
            if (IsAuthController(context) || IsSwaggerRequest(context))
            {
                await _next(context);
                return;
            }

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
            return controllerName == "Auth";
        }

        return false;
    }

    private static bool IsSwaggerRequest(HttpContext context)
    {
        return context.Request.Path.StartsWithSegments("/swagger");
    }



    private static bool IsAuthenticated(HttpContext context)
    {
        var isAuthenticated = context.User.Identity?.IsAuthenticated;
        return isAuthenticated.HasValue && isAuthenticated.Value;
    }
}
