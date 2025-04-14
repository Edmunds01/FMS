using AutoMapper;
using web_api.Exceptions;
using web_api.Repository.Interfaces;
using web_api.Services.Interfaces;

namespace web_api.Services;

public class TransactionService(
    ITransactionRepository transactionRepository,
    IAccountService accountService,
    ICategoryService categoryService,
    IHttpContextAccessor httpContextAccessor,
    IMapper mapper
    ) : BaseService(httpContextAccessor, mapper), ITransactionService
{
    private readonly ITransactionRepository _transactionRepository = transactionRepository;
    private readonly IAccountService _accountService = accountService;
    private readonly ICategoryService _categoryService = categoryService;

    private const int _maxTransactionAmount = 10_000_000;

    public IEnumerable<Dtos.Transaction> GetUserTransactions(long categoryId, DateTime startDate, DateTime endDate)
    {
        var transactions = _transactionRepository.GetUserTransactions(UserId, categoryId, startDate, endDate);

        return _mapper.Map<IEnumerable<Dtos.Transaction>>(transactions);
    }

    public async Task UpsertTransactionAsync(Dtos.Transaction transactionRaw)
    {
        if (transactionRaw.Amount > _maxTransactionAmount)
        { 
             throw new NotAuthorizedException($"create {nameof(Dtos.Transaction)} with balance greater then {_maxTransactionAmount}");
        }

        await _accountService.ValidateIsUserAccountAsync(transactionRaw.AccountId);
        await _categoryService.ValidateIsUserCategoryAsync(transactionRaw.CategoryId);

        var transaction = _mapper.Map<Models.Transaction>(transactionRaw);

        if (transactionRaw.TransactionId == default || transactionRaw.TransactionId == 0)
        {
            transaction.TransactionId = 0;
            transaction.UserId = UserId;

            await _transactionRepository.InsertAsync(transaction);
        }
        else
        {
            await ValidateIsUserTransactionAsync(transactionRaw.TransactionId!.Value);

            await _transactionRepository.UpdateTransaction(transaction);
        }
    }

    public async Task SaveTransactionAccount(long transactionId, long accountId)
    {
        await _accountService.ValidateIsUserAccountAsync(accountId);

        await SaveTransactionAsync(transactionId, transaction => transaction.AccountId = accountId);
    }

    public async Task SaveTransactionCategory(long transactionId, long categoryId)
    {
        await _categoryService.ValidateIsUserCategoryAsync(categoryId);

        await SaveTransactionAsync(transactionId, transaction => transaction.CategoryId = categoryId);
    }

    public async Task SaveTransactionAmount(long transactionId, decimal amount) => await SaveTransactionAsync(transactionId, transaction => transaction.Amount = amount);

    public async Task SaveTransactionComment(long transactionId, string comment) => await SaveTransactionAsync(transactionId, transaction => transaction.Comment = comment);

    public async Task SaveTransactionDate(long transactionId, DateTime newDate) => await SaveTransactionAsync(transactionId, transaction => transaction.CreatedDateTime = newDate);

    public async Task DeleteTransactionAsync(long transactionId)
    {
        await ValidateIsUserTransactionAsync(transactionId);

        await _transactionRepository.DeleteAsync(transactionId);
    }

    private async Task SaveTransactionAsync(long transactionId, Action<Models.Transaction> updateTransaction)
    {
        await ValidateIsUserTransactionAsync(transactionId);

        var transaction = await _transactionRepository.GetByIdStrictAsync(transactionId);

        updateTransaction(transaction);

        await _transactionRepository.SaveChangesAsync();
    }

    private async Task ValidateIsUserTransactionAsync(long transactionId)
    {
        var transaction = await _transactionRepository.GetByIdAsync(transactionId);

        if (transaction == null || transaction.UserId != UserId)
        {
            throw new NotAuthorizedException(nameof(Models.Transaction), transactionId);
        }
    }
}
