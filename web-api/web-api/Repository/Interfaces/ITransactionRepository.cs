using web_api.Models;

namespace web_api.Repository.Interfaces;

public interface ITransactionRepository : IBaseRepository<Transaction>
{
    IEnumerable<Transaction> GetUserTransactions(int userId, long categoryId, DateTime startDate, DateTime endDate);

    IEnumerable<Transaction> GetUserTransactions(int userId);

    Task UpdateTransaction(Transaction transactionRaw);
}
