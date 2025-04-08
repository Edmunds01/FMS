using Microsoft.EntityFrameworkCore;
using web_api.Models;
using web_api.Repository.Interfaces;

namespace web_api.Repository;

public class TransactionRepository(FMSContext context) : BaseRepository<Transaction>(context), ITransactionRepository
{
    private new readonly FMSContext _context = context;

    public IEnumerable<Transaction> GetUserTransactions(int userId, long categoryId, DateTime startDate, DateTime endDate)
    {
        startDate = new DateTime(startDate.Year, startDate.Month, startDate.Day, 0, 0, 0, DateTimeKind.Utc);
        endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59, DateTimeKind.Utc);

        return GetUserTransactions(userId).Where(t => t.CategoryId == categoryId && t.CreatedDateTime >= startDate && t.CreatedDateTime <= endDate);
    }

    public IEnumerable<Transaction> GetUserTransactions(int userId) => _context.Transactions.Where(t => t.UserId == userId);

    public async Task UpdateTransaction(Transaction transactionRaw)
    {
        var transaction = await GetByIdAsync(transactionRaw.TransactionId);
        if (transaction == null)
        {
            throw new NotSupportedException(nameof(transaction));
        }

        transaction.AccountId = transactionRaw.AccountId;
        transaction.CategoryId = transactionRaw.CategoryId;
        transaction.Amount = transactionRaw.Amount;
        transaction.Comment = transactionRaw.Comment;
        transaction.CreatedDateTime = transactionRaw.CreatedDateTime;

        await SaveChangesAsync();
    }
}
