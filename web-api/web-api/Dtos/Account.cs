namespace web_api.Dtos;

public class Account
{
    public required long AccountId { get; set; }
    public required string Name { get; set; }
    public required string Icon { get; set; }
    public required decimal Balance { get; set; }
}
