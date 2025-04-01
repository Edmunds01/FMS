using AutoMapper;
using web_api.Exceptions;
using web_api.Models;
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
        IMapper mapper
    )
        : base(httpContextAccessor, mapper)
    {
        _accountRepository = accountRepository;
        _transactionRepository = transactionRepository;
    }

    public async Task<IEnumerable<Dtos.Account>> GetUserAccountsAsync()
    {
        var accounts = await _accountRepository.GetUserAccountsAsync(UserId);
        var transactions = await _transactionRepository.GetUserTransactions(UserId);

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

    public Task CreateNewAccountAsync(Dtos.Account accountRaw)
    {
        var account = _mapper.Map<Models.Account>(accountRaw);
        account.UserId = UserId;
        account.AccountId = 0;

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
            throw new NotAuthorizedException(nameof(Account), accountId);
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
