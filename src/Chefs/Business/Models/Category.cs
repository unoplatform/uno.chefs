using Chefs.Data;

namespace Chefs.Business;

public record Category
{
    internal Category(CategoryData? category)
    {
        Id = category?.Id;
        UrlIcon = category?.UrlIcon;
        Name = category?.Name;
    }

    public string? Id { get; init; }
    public string? UrlIcon { get; init; }
    public string? Name { get; init; }
}
