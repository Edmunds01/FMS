using Microsoft.EntityFrameworkCore;
using web_api.Models;

namespace web_api.Repository;

public class TransactionRepository : BaseRepository<Transaction>,  ITransactionRepository
{
    private new readonly FMSContext _context;

    public TransactionRepository(FMSContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Transaction>> GetUserTransactions(int userId)
    {
        return await _context.Transactions.Where(t => t.UserId == userId).ToListAsync();
    }
}
