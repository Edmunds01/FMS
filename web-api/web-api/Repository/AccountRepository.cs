using Microsoft.EntityFrameworkCore;
using web_api.Models;

namespace web_api.Repository;

public class AccountRepository : BaseRepository<Account>, IAccountRepository
{
    private new readonly FMSContext _context;

    public AccountRepository(FMSContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Account>> GetUserAccountsAsync(int userId)
    {
        return await _context.Accounts.Where(a => a.UserId == userId).ToListAsync();
    }
}
