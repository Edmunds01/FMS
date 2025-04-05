using Microsoft.EntityFrameworkCore;
using web_api.Models;
using web_api.Repository.Interfaces;

namespace web_api.Repository;

public class AccountRepository(FMSContext context) : BaseRepository<Account>(context), IAccountRepository
{
    private new readonly FMSContext _context = context;

    public IEnumerable<Account> GetUserAccounts(int userId)
    {
        return _context.Accounts.Where(a => a.UserId == userId);
    }
}
