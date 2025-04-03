namespace web_api.Services;

public interface ICategoryService
{
    Task CreateNewCategoryAsync(Dtos.NewCategory categoryRaw);
    Task<IEnumerable<Dtos.Category>> GetUserCategoriesAsync();
    Task ValidateIsUserCategoryAsync(long categoryId);
}
