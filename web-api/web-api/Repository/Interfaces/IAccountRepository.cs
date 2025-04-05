using web_api.Models;

namespace web_api.Repository.Interfaces;

public interface IAccountRepository : IBaseRepository<Account>
{
    IEnumerable<Account> GetUserAccounts(int userId);
}
