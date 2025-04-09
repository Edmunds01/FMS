using Microsoft.AspNetCore.Mvc.Filters;
using web_api.Models;
using web_api.Repository.Interfaces;

namespace web_api.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class AuditLogAttribute(string message) : ActionFilterAttribute
{
    private readonly string _message = message;

    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var auditLogRepository = context.HttpContext.RequestServices.GetService<IAuditLogRepository>();

        if (auditLogRepository == null)
        {
            throw new InvalidOperationException("IAuditLogRepository is not registered in the DI container.");
        }

        var ipAddress = context.HttpContext.Connection.RemoteIpAddress?.ToString();
        var userId = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

        await auditLogRepository.InsertAsync(new AuditLog
        {
            Action = _message,
            IpAdress = ipAddress,
            UserId = userId != null ? int.Parse(userId) : null,
            ActionDttm = DateTime.UtcNow
        });

        await next();
    }
}
