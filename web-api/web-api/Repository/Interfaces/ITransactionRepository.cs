using web_api.Models;

namespace web_api.Repository.Interfaces;

public interface ITransactionRepository : IBaseRepository<Transaction>
{
    public IEnumerable<Transaction> GetUserTransactions(int userId, long categoryId);
    public IEnumerable<Transaction> GetUserTransactions(int userId);
}
