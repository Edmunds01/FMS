namespace web_api.Services;

public interface IAccountService
{
    Task<IEnumerable<Dtos.Account>> GetUserAccountsAsync();

    Task CreateNewAccountAsync(Dtos.Account accountRaw);

    Task SaveAccountIconAsync(long accountId, string iconName);

    Task SaveAccountNameAsync(long accountId, string name);

    Task DeleteAccountAsync(long accountId);

    Task ValidateIsUserAccountAsync(long accountId);
}
