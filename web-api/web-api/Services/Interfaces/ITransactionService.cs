namespace web_api.Services.Interfaces;

public interface ITransactionService
{
    IEnumerable<Dtos.Transaction> GetUserTransactions(long categoryId);

    Task CreateNewTransactionAsync(Dtos.NewTransaction transactionRaw);

    Task SaveTransactionAccount(long transactionId, long accountId);

    Task SaveTransactionCategory(long transactionId, long categoryId);

    Task SaveTransactionAmount(long transactionId, decimal amount);

    Task SaveTransactionComment(long transactionId, string comment);

    Task SaveTransactionDate(long transactionId, DateTime newDate);

    Task DeleteTransaction(long transactionId);
}
