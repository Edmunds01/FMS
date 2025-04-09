using web_api.Models;

namespace web_api.Repository.Interfaces;

public interface IUserRepository: IBaseRepository<User>
{
    Task<User> AddUserAsync(User user);

    Task<User?> GetUserAsync(string email);
}
