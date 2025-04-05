using AutoMapper;
using web_api.Repository;
using web_api.Services.Interfaces;

namespace web_api.Services;

public class CategoryService(
    ITransactionRepository transactionRepository,
    ICategoryRepository categoryRepository,
    IHttpContextAccessor httpContextAccessor,
    IConfiguration configuration,
    IHostEnvironment env,
    IMapper mapper
    ) : BaseService(httpContextAccessor, mapper, configuration, env), ICategoryService
{
    private readonly ITransactionRepository _transactionRepository = transactionRepository;
    private readonly ICategoryRepository _categoryRepository = categoryRepository;

    public async Task<IEnumerable<Dtos.Category>> GetUserCategoriesAsync()
    {
        var categories = await _categoryRepository.GetUserCategoriesAsync(UserId);
        
        return _mapper.Map<IEnumerable<Dtos.Category>>(categories);
    }

    public async Task SaveCategoryIconAsync(long categoryId, string icon)
    {
        await SaveCategoryAsync(categoryId, category =>
        {
            category.Icon = icon;
        });
    }

    public async Task SaveCategoryNameAsync(long categoryId, string name)
    {
        await SaveCategoryAsync(categoryId, category =>
        {
            category.Name = name;
        });
    }

    public Task CreateNewCategoryAsync(Dtos.NewCategory categoryRaw)
    {
        var category = _mapper.Map<Models.Category>(categoryRaw);
        category.UserId = UserId;
        category.CategoryId = 0;

        return _categoryRepository.InsertAsync(category);
    }

    public async Task DeleteCategoryAsync(long categoryId)
    {
        await ValidateIsUserCategoryAsync(categoryId);

        await _categoryRepository.DeleteAsync(categoryId);
    }

    public async Task ValidateIsUserCategoryAsync(long categoryId)
    {
        var account = await _categoryRepository.GetByIdAsync(categoryId);

        if (account == null || account.UserId != UserId)
        {
            throw new NotSupportedException("Category not found or wrong user");
        }
    }

    private async Task SaveCategoryAsync(long categoryId, Action<Models.Category> updateCategory)
    {
        await ValidateIsUserCategoryAsync(categoryId);

        var category = await _categoryRepository.GetByIdStrictAsync(categoryId);

        updateCategory(category);

        await _transactionRepository.SaveChanges();
    }
}
