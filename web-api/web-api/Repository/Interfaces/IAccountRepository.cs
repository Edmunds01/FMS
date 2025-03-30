using web_api.Models;

namespace web_api.Repository;

public interface IAccountRepository
{
    IEnumerable<Account> GetUserAccounts(int userId);
}
