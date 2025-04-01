using web_api.Models;
using web_api.Repository.Interfaces;

namespace web_api.Repository;

public interface IAccountRepository : IBaseRepository<Account>
{
    Task<IEnumerable<Account>> GetUserAccountsAsync(int userId);
}
