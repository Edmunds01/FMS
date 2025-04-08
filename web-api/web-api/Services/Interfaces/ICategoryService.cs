namespace web_api.Services.Interfaces;

public interface ICategoryService
{
    Task CreateNewCategoryAsync(Dtos.NewCategory categoryRaw);

    IEnumerable<Dtos.Category> GetUserCategories(DateTime startDate, DateTime endDate);

    Task ValidateIsUserCategoryAsync(long categoryId);

    Task SaveCategoryIconAsync(long categoryId, string icon);

    Task SaveCategoryNameAsync(long categoryId, string name);

    Task DeleteCategoryAsync(long categoryId);
}
