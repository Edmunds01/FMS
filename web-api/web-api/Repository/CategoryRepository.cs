using Microsoft.EntityFrameworkCore;
using web_api.Models;
using web_api.Repository.Interfaces;

namespace web_api.Repository;

public class CategoryRepository(FMSContext context) : BaseRepository<Category>(context), ICategoryRepository
{
    private new readonly FMSContext _context = context;

    public IEnumerable<Category> GetUserCategories(int userId) => _context.Categories.Where(c => c.UserId == userId);

    public async Task DeleteAsync(long categoryId)
    {
        var category = await GetByIdAsync(categoryId) ?? throw new NotSupportedException("Category does not exist");

        if (await _context.Transactions.AnyAsync(t => t.CategoryId == categoryId))
        {
            throw new NotSupportedException("Cannot delete category with transactions");
        }

        _dbSet.Remove(category);
        await _context.SaveChangesAsync();
    }
}
