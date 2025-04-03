using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using web_api.Exceptions;

namespace web_api.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class NotAuthorizedExceptionFilterAttribute : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        if (context.Exception is NotAuthorizedException)
        {
            context.Result = new ObjectResult(new
            {
                context.Exception.Message
            })
            {
                StatusCode = StatusCodes.Status403Forbidden
            };

            context.ExceptionHandled = true;
        }
    }
}
