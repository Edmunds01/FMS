using web_api.Models;

namespace web_api.Repository;

public interface IUserRepository
{
    Task<User> AddUserAsync(User user);
    User? GetUser(string email);
}
