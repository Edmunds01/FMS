namespace web_api.Services.Interfaces;

public interface ITransactionService
{
    Task CreateNewTransactionAsync(Dtos.Transaction transactionRaw);
    Task SaveTransactionAccount(long transactionId, long newAccountId);
    Task SaveTransactionCategory(long transactionId, long newCategoryId);
    Task SaveTransactionAmount(long transactionId, decimal newAmount);
    Task SaveTransactionComment(long transactionId, string comment);
    Task SaveTransactionDate(long transactionId, DateTime newDate);
    Task DeleteTransaction(long transactionId);
}
