using web_api.Models;

namespace web_api.Repository;

public interface IUserRepository
{
    void AddUser(User user);
    User? GetUser(string email);
}
