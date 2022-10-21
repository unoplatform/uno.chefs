namespace Chefs.Data;

public record CategoryData
{
    public int? Id { get; init; }
    public string? UrlIcon { get; init; }
    public string? Name { get; init; }
}
