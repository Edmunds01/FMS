namespace web_api.Dtos;

public class NewTransaction
{
    public required long AccountId { get; set; }
    public required long CategoryId { get; set; }
    public required string? Comment { get; set; }
    public required decimal Amount { get; set; }
    public required DateTime CreatedDateTime { get; set; }
}
