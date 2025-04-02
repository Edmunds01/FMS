using AutoMapper;
using web_api.Exceptions;
using web_api.Repository;

namespace web_api.Services;

public class AccountService : BaseService, IAccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly ITransactionRepository _transactionRepository;

    public AccountService
    (
        IAccountRepository accountRepository,
        ITransactionRepository transactionRepository,
        IHttpContextAccessor httpContextAccessor,
        IHostEnvironment env,
        IConfiguration configuration,
        IMapper mapper
    )
        : base(httpContextAccessor, mapper, configuration, env)
    {
        _accountRepository = accountRepository;
        _transactionRepository = transactionRepository;
    }

    public async Task<IEnumerable<Dtos.Account>> GetUserAccountsAsync()
    {
        var accounts = await _accountRepository.GetUserAccountsAsync(UserId);
        var transactions = await _transactionRepository.GetUserTransactionsAsync(UserId);

        return accounts.Select(a => new Dtos.Account
        {
            AccountId = a.AccountId,
            Name = a.Name,
            Icon = a.Icon,
            Balance = transactions.Where(t => t.AccountId == a.AccountId).Sum(t => t.Amount) + a.InitialBalance,
            ShowDeleteButton = !transactions.Any(t => t.AccountId == a.AccountId)
        });
    }

    public async Task SaveAccountNameAsync(long accountId, string name)
    {
        await SaveAccountAsync(accountId, category =>
        {
            category.Icon = name;
        });
    }

    public async Task SaveAccountIconAsync(long accountId, string iconName)
    {
        await SaveAccountAsync(accountId, category =>
        {
            category.Icon = iconName;
        });
    }

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

        await _transactionRepository.SaveChanges();
    }
}
