namespace web_api.Dtos;

public class Category
{
    public required long CategoryId { get; set; }
    public required string Name { get; set; }
    public required string Icon { get; set; }
    public CategoryType Type { get; set; }
    public required bool ShowDeleteButton { get; set; }
    public required decimal SumOfTransactions { get; set; }
}
