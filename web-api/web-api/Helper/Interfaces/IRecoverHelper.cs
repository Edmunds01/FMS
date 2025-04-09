using web_api.Models;
using web_api.Repository.Interfaces;

namespace web_api.Helper.Interfaces;

public interface IRecoverHelper
{
    void SendRecoveryEmail(string email);

    bool ValidateRecoveryToken(Guid token, out string? email);
}
