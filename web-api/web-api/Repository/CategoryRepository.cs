using Microsoft.EntityFrameworkCore;
using web_api.Models;
using web_api.Repository.Interfaces;

namespace web_api.Repository;

public class CategoryRepository(FMSContext context) : BaseRepository<Category>(context), ICategoryRepository
{
    private new readonly FMSContext _context = context;

    public IEnumerable<Category> GetUserCategories(int userId)
    {
        return _context.Categories.Where(c => c.UserId == userId);
    }
}
