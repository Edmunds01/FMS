namespace web_api.Services;

public interface IAccountService
{
    IEnumerable<Dtos.Account> GetUserAccounts();

    Task<bool> SaveAccountIconAsync(long accountId, string accountIcon);

    Task<bool> SaveAccountNameAsync(long accountId, string accountName);

    Task CreateNewAccountAsync(Dtos.Account accountRaw);

    Task<bool> DeleteAccountAsync(long accountId);
}
