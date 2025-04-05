using Microsoft.EntityFrameworkCore;
using web_api.Models;
using web_api.Repository.Interfaces;

namespace web_api.Repository;

public class TransactionRepository(FMSContext context) : BaseRepository<Transaction>(context), ITransactionRepository
{
    private new readonly FMSContext _context = context;

    public IEnumerable<Transaction> GetUserTransactions(int userId, long categoryId)
    {
         return GetUserTransactions(userId).Where(t => t.CategoryId == categoryId);
    }

    public IEnumerable<Transaction> GetUserTransactions(int userId)
    {
        return _context.Transactions.Where(t => t.UserId == userId);
    }
}
