namespace web_api.Repository.Interfaces;

public interface IBaseRepository<T> where T : class
{
    Task SaveChanges();
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(object id);
    Task<T> GetByIdStrictAsync(object id);
    Task InsertAsync(T entity);
    Task DeleteAsync(object id);
}
