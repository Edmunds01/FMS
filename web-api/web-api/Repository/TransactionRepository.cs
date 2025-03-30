using web_api.Models;

namespace web_api.Repository;

public class TransactionRepository : ITransactionRepository
{
    private readonly FMSContext _context;

    public TransactionRepository(FMSContext context)
    {
        _context = context;
    }

    public IEnumerable<Transaction> GetUserTransactions(int userId)
    {
        return _context.Transactions.Where(a => a.UserId == userId);
    }
}
