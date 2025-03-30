using web_api.Repository;

namespace web_api.Services;

public class AccountService : IAccountService
{

    private readonly IAccountRepository _accountRepository;
    private readonly ITransactionRepository _transactionRepository;

    public AccountService(IAccountRepository accountRepository, ITransactionRepository transactionRepository)
    {
        _accountRepository = accountRepository;
        _transactionRepository = transactionRepository;
    }

    public IEnumerable<Dtos.Account> GetUserAccounts(int userId)
    {
        var accounts = _accountRepository.GetUserAccounts(userId).ToList();
        var transaction = _transactionRepository.GetUserTransactions(userId).ToList();

        return accounts.Select(a => new Dtos.Account
        {
            AccountId = a.AccountId,
            Name = a.Name,
            Icon = a.Icon,
            Balance = transaction.Where(t => t.AccountId == a.AccountId).Sum(t => t.Amount) + a.InitialBalance
        });
    }
}
