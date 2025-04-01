namespace web_api.MiddleWere;

public class ConditionalAuthorizeMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IHostEnvironment _env;

    public ConditionalAuthorizeMiddleware(RequestDelegate next, IHostEnvironment env)
    {
        _next = next;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {   
        var endpoint = context.GetEndpoint();
        if (endpoint != null)
        {
            var controllerName = endpoint.Metadata.GetMetadata<Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor>()?.ControllerName;
            if (controllerName == "Login")
            {
                await _next(context);
                return;
            }
        }

        if (_env.IsProduction() && !context.User.Identity.IsAuthenticated)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        await _next(context);
    }
}