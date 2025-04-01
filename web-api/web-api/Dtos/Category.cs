namespace web_api.Dtos;

public class Category
{
    public long? CategoryId { get; set; }
    public required string Name { get; set; }
    public required string Icon { get; set; }
    public CategoryType Type { get; set; }
    public bool? ShowDeleteButton { get; set; }
}
