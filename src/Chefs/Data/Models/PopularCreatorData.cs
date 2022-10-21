using System.Collections.Immutable;

namespace Chefs.Data;

public record PopularCreatorData
{
    public string? UrlProfileImage { get; init; }
    public string? FullName { get; init; }
    public long? Followers { get; init; }
    public long? Following { get; init; }
    public IImmutableList<RecipeData>? Recipes { get; init; }
    public IImmutableList<CookbookData>? Cookbooks { get; init; }
}
