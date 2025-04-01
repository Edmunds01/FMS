using AutoMapper;
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

    public IEnumerable<Dtos.Account> GetUserAccounts()
    {
        var accounts = _accountRepository.GetUserAccounts(UserId).ToList();
        var transactions = _transactionRepository.GetUserTransactions(UserId).ToList();

        return accounts.Select(a => new Dtos.Account
        {
            AccountId = a.AccountId,
            Name = a.Name,
            Icon = a.Icon,
            Balance = transactions.Where(t => t.AccountId == a.AccountId).Sum(t => t.Amount) + a.InitialBalance,
            ShowDeleteButton = !transactions.Any(t => t.AccountId == a.AccountId)
        });
    }

    public Task<bool> SaveAccountIconAsync(long accountId, string accountIcon)
    {
        return _accountRepository.SaveAccountIconAsync(accountId, accountIcon, UserId);
    }

    public Task<bool> SaveAccountNameAsync(long accountId, string accountName)
    {
        return _accountRepository.SaveAccountNameAsync(accountId, accountName, UserId);
    }

    public Task CreateNewAccountAsync(Dtos.Account accountRaw)
    {
        var account = _mapper.Map<Models.Account>(accountRaw);
        account.UserId = UserId;
        account.AccountId = 0;

        return _accountRepository.CreateNewAccountAsync(account);
    }
    
    public Task<bool> DeleteAccountAsync(long accountId)
    {
        return _accountRepository.DeleteAccountAsync(accountId, UserId);
    }
}
