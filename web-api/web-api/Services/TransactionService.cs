using AutoMapper;
using web_api.Repository;
using web_api.Services.Interfaces;

namespace web_api.Services;

public class TransactionService : BaseService, ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IAccountService _accountService;
    private readonly ICategoryService _categoryService;

    public TransactionService
    (
        ITransactionRepository transactionRepository,
        IAccountService accountService,
        ICategoryService categoryService,
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper
    )
        : base(httpContextAccessor, mapper)
    {
        _transactionRepository = transactionRepository;
        _accountService = accountService;
        _categoryService = categoryService;
    }

    public async Task CreateNewTransactionAsync(Dtos.Transaction transactionRaw)
    {
        await _accountService.ValidateIsUserAccountAsync(transactionRaw.AccountId);

        var transaction = _mapper.Map<Models.Transaction>(transactionRaw);
        transaction.UserId = UserId;
        transaction.TransactionId = 0;

        await _transactionRepository.InsertAsync(transaction);
    }

    public async Task SaveTransactionAccount(long transactionId, long newAccountId)
    {
        await _accountService.ValidateIsUserAccountAsync(newAccountId);

        await SaveTransactionAsync(transactionId, transaction =>
        {
            transaction.AccountId = newAccountId;
        });
    }

    public async Task SaveTransactionCategory(long transactionId, long newCategoryId)
    {
        await _categoryService.ValidateIsUserCategoryAsync(newCategoryId);

        await SaveTransactionAsync(transactionId, transaction =>
        {
            transaction.CategoryId = newCategoryId;
        });
    }

    public async Task SaveTransactionAmount(long transactionId, decimal newAmount)
    {
        await SaveTransactionAsync(transactionId, transaction =>
        {
            transaction.Amount = newAmount;
        });
    }

    public async Task SaveTransactionComment(long transactionId, string comment)
    {
        await SaveTransactionAsync(transactionId, transaction =>
        {
            transaction.Comment = comment;
        });
    }

    public async Task SaveTransactionDate(long transactionId, DateTime newDate)
    {
        await SaveTransactionAsync(transactionId, transaction =>
        {
            transaction.CreatedDateTime = newDate;
        });
    }

    public async Task DeleteTransaction(long transactionId)
    {
        await ValidateIsUserTransactionAsync(transactionId);

        await _transactionRepository.DeleteAsync(transactionId);
    }

    private async Task SaveTransactionAsync(long transactionId, Action<Models.Transaction> updateTransaction)
    {
        await ValidateIsUserTransactionAsync(transactionId);

        var transaction = await _transactionRepository.GetByIdStrictAsync(transactionId);

        updateTransaction(transaction);

        await _transactionRepository.SaveChanges();
    }

    private async Task ValidateIsUserTransactionAsync(long transactionId)
    {
        var transaction = await _transactionRepository.GetByIdAsync(transactionId);

        if (transaction == null || transaction.UserId != UserId)
        {
            throw new NotSupportedException("Transaction not found or wrong user");
        }
    }
}
