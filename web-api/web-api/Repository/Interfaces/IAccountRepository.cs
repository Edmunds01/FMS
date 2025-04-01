namespace web_api.Repository;

public interface IAccountRepository
{
    IEnumerable<Models.Account> GetUserAccounts(int userId);
    Task<bool> SaveAccountIconAsync(long accountId, string accountIcon, int userId);
    Task<bool> SaveAccountNameAsync(long accountId, string accountName, int userId);
    Task CreateNewAccountAsync(Models.Account account);
    Task<bool> DeleteAccountAsync(long accountId, int userId);
}
