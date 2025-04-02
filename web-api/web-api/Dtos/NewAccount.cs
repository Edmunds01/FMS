namespace web_api.Dtos;

public class NewAccount
{
    public required string Name { get; set; }
    public required string Icon { get; set; }
    public required decimal Balance { get; set; }
}
