using Microsoft.EntityFrameworkCore;
using web_api.Models;
using web_api.Repository.Interfaces;

namespace web_api.Repository;

public class TransactionRepository(FMSContext context) : BaseRepository<Transaction>(context), ITransactionRepository
{
    private new readonly FMSContext _context = context;

    public IEnumerable<Transaction> GetUserTransactions(int userId, long categoryId) => GetUserTransactions(userId).Where(t => t.CategoryId == categoryId);

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
