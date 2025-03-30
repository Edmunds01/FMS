using web_api.Models;

namespace web_api.Repository;

public interface ITransactionRepository
{
    IEnumerable<Transaction> GetUserTransactions(int userId);
}
