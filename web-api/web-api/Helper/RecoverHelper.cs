using System.Diagnostics;
using web_api.Helper.Interfaces;

namespace web_api.Helper;

public class RecoverHelper() : IRecoverHelper
{
    private readonly Dictionary<Guid, string> _recoveryValues = [];

    public void SendRecoveryEmail(string email)
    {
        var token = Guid.NewGuid();
        _recoveryValues.Add(token, email);
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