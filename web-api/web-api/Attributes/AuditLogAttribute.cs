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
        _ = Task.Run(async () =>
        {
            using var scope = context.HttpContext.RequestServices.CreateScope();
            var auditLogRepository = scope.ServiceProvider.GetRequiredService<IAuditLogRepository>();

            var ipAddress = context.HttpContext.Connection.RemoteIpAddress?.ToString();
            var userId = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

            try
            {
                await auditLogRepository.InsertAsync(new AuditLog
                {
                    Action = _message,
                    IpAdress = ipAddress,
                    UserId = userId != null ? int.Parse(userId) : null,
                    ActionDttm = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error logging audit: {ex.Message}");
            }
        });

        await next();
    }
}
