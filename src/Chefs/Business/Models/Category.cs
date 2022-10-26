using Chefs.Data;
using System.Net;

namespace Chefs.Business;

public record Category
{
    internal Category(CategoryData? category)
    {
        Id = category?.Id;
        UrlIcon = category?.UrlIcon;
        Name = category?.Name;
    }

    public int? Id { get; init; }
    public string? UrlIcon { get; init; }
    public string? Name { get; init; }

    internal CategoryData ToData() => new()
    {
        Id = Id,
        UrlIcon = UrlIcon,
        Name = Name
    };
}
