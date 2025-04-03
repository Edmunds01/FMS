namespace web_api.Dtos;

public class NewCategory
{
    public required string Name { get; set; }
    public required string Icon { get; set; }
    public CategoryType Type { get; set; }
}
