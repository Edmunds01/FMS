namespace web_api.Services.Interfaces;

public interface ICategoryService
{
    Task CreateNewCategoryAsync(Dtos.NewCategory categoryRaw);

    Task<IEnumerable<Dtos.Category>> GetUserCategoriesAsync();

    Task ValidateIsUserCategoryAsync(long categoryId);

    Task SaveCategoryIconAsync(long categoryId, string icon);

    Task SaveCategoryNameAsync(long categoryId, string name);

    Task DeleteCategoryAsync(long categoryId);
}
