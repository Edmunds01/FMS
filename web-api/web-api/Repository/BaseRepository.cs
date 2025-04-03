using Microsoft.EntityFrameworkCore;
using web_api.Repository.Interfaces;

namespace web_api.Repository;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    protected readonly DbContext _context;
    protected readonly DbSet<T> _dbSet;

    public BaseRepository(DbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _dbSet = _context.Set<T>();
    }

    public Task SaveChanges() => _context.SaveChangesAsync();

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<T?> GetByIdAsync(object id)
    {
        return await _dbSet.FindAsync(id);
    }
    
    public async Task<T> GetByIdStrictAsync(object id)
    {
        return await _dbSet.FindAsync(id) ?? throw new NotSupportedException("Use GetByIdAsync() if not sure that object exists in db");
    }

    public async Task InsertAsync(T entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(object id)
    {
        var entity = await GetByIdAsync(id);

        if (entity == null)
        {
            throw new NotSupportedException(nameof(entity));
        }

        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
    }
}