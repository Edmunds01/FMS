using AutoMapper;
using web_api.Exceptions;
using web_api.Repository.Interfaces;
using web_api.Services.Interfaces;

namespace web_api.Services;

public class AccountService(
    IAccountRepository accountRepository,
    ITransactionRepository transactionRepository,
    ICategoryRepository categoryRepository,
    IHttpContextAccessor httpContextAccessor,
    IHostEnvironment env,
    IConfiguration configuration,
    IMapper mapper
    ) : BaseService(httpContextAccessor, mapper, configuration, env), IAccountService
{
    private readonly IAccountRepository _accountRepository = accountRepository;
    private readonly ITransactionRepository _transactionRepository = transactionRepository;
    private readonly ICategoryRepository _categoryRepository = categoryRepository;

    public IEnumerable<Dtos.Account> GetUserAccounts()
    {
        var accounts = _accountRepository.GetUserAccounts(UserId).ToList();
        var transactions = _transactionRepository.GetUserTransactions(UserId).ToList();
        var categories = _categoryRepository.GetUserCategories(UserId).ToList();

        return accounts.Select(a => new Dtos.Account
        {
            AccountId = a.AccountId,
            Name = a.Name,
            Icon = a.Icon,
            Balance = transactions.Where(t => t.AccountId == a.AccountId).Sum(t => categories.First(c => c.CategoryId == t.CategoryId).Type == 1 ? t.Amount : (-1 * t.Amount)) + a.InitialBalance,
            ShowDeleteButton = !transactions.Any(t => t.AccountId == a.AccountId)
        });
    }

    public async Task SaveAccountNameAsync(long accountId, string name) => await SaveAccountAsync(accountId, category => category.Name = name);

    public async Task SaveAccountIconAsync(long accountId, string icon) => await SaveAccountAsync(accountId, category => category.Icon = icon);

    public Task CreateNewAccountAsync(Dtos.NewAccount accountRaw)
    {
        var account = _mapper.Map<Models.Account>(accountRaw);
        account.UserId = UserId;

        return _accountRepository.InsertAsync(account);
    }

    public async Task DeleteAccountAsync(long accountId)
    {
        await ValidateIsUserAccountAsync(accountId);

        await _accountRepository.DeleteAsync(accountId);
    }

    public async Task ValidateIsUserAccountAsync(long accountId)
    {
        var account = await _accountRepository.GetByIdAsync(accountId);

        if (account == null || account.UserId != UserId)
        {
            throw new NotAuthorizedException(nameof(Models.Account), accountId);
        }
    }

    private async Task SaveAccountAsync(long accountId, Action<Models.Account> updateAccount)
    {
        await ValidateIsUserAccountAsync(accountId);

        var account = await _accountRepository.GetByIdStrictAsync(accountId);

        updateAccount(account);

        await _transactionRepository.SaveChangesAsync();
    }
}
