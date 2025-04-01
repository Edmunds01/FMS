namespace web_api.Services;

public interface ICategoryService
{
    Task ValidateIsUserCategoryAsync(long categoryId);
}
