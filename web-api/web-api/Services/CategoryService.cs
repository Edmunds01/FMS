using AutoMapper;
using web_api.Exceptions;
using web_api.Repository.Interfaces;
using web_api.Services.Interfaces;

namespace web_api.Services;

public class CategoryService(
    ITransactionRepository transactionRepository,
    ICategoryRepository categoryRepository,
    IHttpContextAccessor httpContextAccessor,
    IMapper mapper
    ) : BaseService(httpContextAccessor, mapper), ICategoryService
{
    private readonly ITransactionRepository _transactionRepository = transactionRepository;
    private readonly ICategoryRepository _categoryRepository = categoryRepository;

    public IEnumerable<Dtos.Category> GetUserCategories(DateTime startDate, DateTime endDate)
    {
        startDate = new DateTime(startDate.Year, startDate.Month, startDate.Day, 0, 0, 0, DateTimeKind.Utc);
        endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59, DateTimeKind.Utc);

        var categoriesRaw = _categoryRepository.GetUserCategories(UserId).ToList();
        var transactions = _transactionRepository.GetUserTransactions(UserId).ToList();
        var categories = _mapper.Map<IEnumerable<Dtos.Category>>(categoriesRaw);

        return categories.Select(category =>
        {
            category.ShowDeleteButton = !transactions.Any(t => t.CategoryId == category.CategoryId);
            category.SumOfTransactions = transactions.Where(t => t.CategoryId == category.CategoryId && t.CreatedDateTime >= startDate && t.CreatedDateTime <= endDate).Sum(t => t.Amount);

            return category;
        });
    }

    public async Task SaveCategoryIconAsync(long categoryId, string icon) =>
        await SaveCategoryAsync(categoryId, category => category.Icon = icon);

    public async Task SaveCategoryNameAsync(long categoryId, string name) =>
        await SaveCategoryAsync(categoryId, category => category.Name = name);

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
            throw new NotAuthorizedException(nameof(Models.Category), categoryId);
        }
    }

    private async Task SaveCategoryAsync(long categoryId, Action<Models.Category> updateCategory)
    {
        await ValidateIsUserCategoryAsync(categoryId);

        var category = await _categoryRepository.GetByIdStrictAsync(categoryId);

        updateCategory(category);

        await _transactionRepository.SaveChangesAsync();
    }
}
