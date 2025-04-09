namespace web_api.Helper.Interfaces;

public interface IRecoverHelper
{
    Task SendRecoveryEmailAsync(string email);

    bool ValidateRecoveryToken(Guid token, out string? email);
}
