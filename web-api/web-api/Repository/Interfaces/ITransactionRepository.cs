using web_api.Models;
using web_api.Repository.Interfaces;

namespace web_api.Repository;

public interface ITransactionRepository : IBaseRepository<Transaction>
{
    public Task<IEnumerable<Transaction>> GetUserTransactions(int userId);
}
