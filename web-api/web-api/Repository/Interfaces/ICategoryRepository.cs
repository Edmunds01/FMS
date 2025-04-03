using web_api.Models;
using web_api.Repository.Interfaces;

namespace web_api.Repository;

public interface ICategoryRepository : IBaseRepository<Category>
{
    Task<IEnumerable<Category>> GetUserCategoriesAsync(int userId);
}
