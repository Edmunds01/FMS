using web_api.Models;

namespace web_api.Repository;

public class AccountRepository : IAccountRepository
{
    private readonly FMSContext _context;

    public AccountRepository(FMSContext context)
    {
        _context = context;
    }

    public IEnumerable<Account> GetUserAccounts(int userId)
    {
        return _context.Accounts.Where(a => a.UserId == userId);
    }

    public async Task<bool> SaveAccountIconAsync(long accountId, string accountIcon, int userId)
    {
        var account = await _context.Accounts.FindAsync(accountId);

        if (account == null || account.UserId != userId)
        {
            return false;
        }

        account.Icon = accountIcon;
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> SaveAccountNameAsync(long accountId, string accountName, int userId)
    {
        var account = await _context.Accounts.FindAsync(accountId);

        if (account == null || account.UserId != userId)
        {
            return false;
        }

        account.Name = accountName;
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task CreateNewAccountAsync(Account account)
    {
        await _context.Accounts.AddAsync(account);

        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAccountAsync(long accountId, int userId)
    {
        var account = _context.Accounts.FirstOrDefault(a => a.AccountId == accountId && a.UserId == userId);

        if (account == null)
        {
            return false;
        }

        _context.Remove(account);

        await _context.SaveChangesAsync();

        return true;
    }
}
