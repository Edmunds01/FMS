using Microsoft.EntityFrameworkCore;
using web_api.Models;

namespace web_api.Repository;

public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
{
    private new readonly FMSContext _context;

    public CategoryRepository(FMSContext context) : base(context)
    {
        _context = context;
    }
}
