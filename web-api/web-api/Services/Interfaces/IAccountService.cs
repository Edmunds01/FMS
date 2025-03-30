namespace web_api.Services;

public interface IAccountService
{
    IEnumerable<Dtos.Account> GetUserAccounts(int userId);
}
