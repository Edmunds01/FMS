namespace web_api.Services.Interfaces;

public interface IUserService
{
    Task<string> GetUserEmailAsync();
    Task ChangeUserPasswordAsync(string oldPassword, string newPassword);
}
