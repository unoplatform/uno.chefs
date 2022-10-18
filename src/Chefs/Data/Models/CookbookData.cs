using System.Collections.Immutable;

namespace Chefs.Data;

public record CookbookData
{
    public string? Name { get; init; }
    public int PinsNumber { get; init; }
    public IImmutableList<RecipeData>? Recipes { get; init; }
    public bool Save { get; init; }
}
