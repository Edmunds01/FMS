using AutoMapper;
using web_api.Repository;

namespace web_api.Services;

public class CategoryService : BaseService, ICategoryService
{
    private readonly IAccountRepository _accountRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService
    (
        IAccountRepository accountRepository,
        ITransactionRepository transactionRepository,
        ICategoryRepository categoryRepository,
        IHttpContextAccessor httpContextAccessor,
        IConfiguration configuration,
        IHostEnvironment env,
        IMapper mapper
    )
        : base(httpContextAccessor, mapper, configuration, env)
    {
        _accountRepository = accountRepository;
        _transactionRepository = transactionRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task SaveCategoryIcon(long categoryId, string iconName)
    {

        await SaveCategoryAsync(categoryId, category =>
        {
            category.Icon = iconName;
        });
    }

    public async Task SaveCategoryName(long categoryId, string name)
    {
        await SaveCategoryAsync(categoryId, category =>
        {
            category.Name = name;
        });
    }

    public Task CreateNewCategoryAsync(Dtos.Category categoryRaw)
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
