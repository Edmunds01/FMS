namespace web_api.Dtos;

public class Transaction
{
    public long? TransactionId { get; set; }
    public required long AccountId { get; set; }
    public required long CategoryId { get; set; }
    public required string? Comment { get; set; }
    public required decimal Amount { get; set; }
    public required DateTime CreatedDateTime { get; set; }
}
