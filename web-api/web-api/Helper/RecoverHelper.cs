using System.Diagnostics;
using web_api.Helper.Interfaces;

namespace web_api.Helper;

public class RecoverHelper(IConfiguration configuration) : IRecoverHelper
{
    private readonly Dictionary<Guid, string> _recoveryValues = [];

    private readonly IConfiguration _configuration = configuration;

    public async Task SendRecoveryEmailAsync(string email)
    {
        var token = Guid.NewGuid();
        _recoveryValues.Add(token, email);

        var siteUrl = _configuration["SiteUrl"];
        if (string.IsNullOrEmpty(siteUrl))
        {
            throw new InvalidOperationException("SiteUrl is not configured.");
        }

        var recoveryLink = $"{siteUrl}/change-password?token={token}&email={email}";
        var text = $"Lai atjaunotu paroli, lūdzu, sekojiet šai saitei un izveidojiet jaunu paroli: \n{recoveryLink}";

        await EmailHelper.SendEmailAsync(email, "Paroles atjaunošana", text);
        Debug.WriteLine(@$"http://localhost:5173/change-password?token={token}&email={email}");
    }

    public bool ValidateRecoveryToken(Guid token, out string? email)
    {
        if (_recoveryValues.TryGetValue(token, out email))
        {
            _recoveryValues.Remove(token);
            return true;
        }

        email = null;
        return false;
    }
}