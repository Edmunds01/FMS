using web_api.Models;

namespace web_api.Repository.Interfaces;

public interface ICategoryRepository : IBaseRepository<Category>
{
    IEnumerable<Category> GetUserCategories(int userId);

    Task DeleteAsync(long categoryId);
}
