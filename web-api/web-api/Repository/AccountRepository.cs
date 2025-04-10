using Microsoft.EntityFrameworkCore;
using web_api.Models;
using web_api.Repository.Interfaces;

namespace web_api.Repository;

public class AccountRepository(FMSContext context) : BaseRepository<Account>(context), IAccountRepository
{
    private new readonly FMSContext _context = context;

    public IEnumerable<Account> GetUserAccounts(int userId) => _context.Accounts.Where(a => a.UserId == userId);

    public async Task DeleteAsync(long accountId)
    {
        var account = await GetByIdAsync(accountId) ?? throw new NotSupportedException("Account does not exists");

        if (await _context.Transactions.AnyAsync(t => t.AccountId == accountId))
        {
            throw new NotSupportedException("Cannot delete account with transactions");
        }

        _dbSet.Remove(account);
        await _context.SaveChangesAsync();
    }
}
