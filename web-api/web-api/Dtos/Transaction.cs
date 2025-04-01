namespace web_api.Dtos;

public class Transaction
{
    public long TransactionId { get; set; }
    public long AccountId { get; set; }
    public long CategoryId { get; set; }
    public string? Comment { get; set; }
    public decimal Amount { get; set; }
    public DateTime CreatedDateTime { get; set; }
}
