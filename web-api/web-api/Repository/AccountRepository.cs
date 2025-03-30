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
}
