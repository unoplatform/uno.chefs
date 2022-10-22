using System.Collections.Immutable;

namespace Chefs.Data;

public class PopularCreatorData
{
    public string? UrlProfileImage { get; set; }
    public string? FullName { get; set; }
    public long? Followers { get; set; }
    public long? Following { get; set; }
    public IImmutableList<RecipeData>? Recipes { get; set; }
    public IImmutableList<CookbookData>? Cookbooks { get; set; }
}
