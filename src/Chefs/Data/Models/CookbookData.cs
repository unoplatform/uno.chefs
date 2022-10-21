using System.Collections.Immutable;

namespace Chefs.Data;

public record CookbookData
{
    public int? Id { get; init; } 
    public int? UserId { get; init; }
    public string? Name { get; init; }
    public int PinsNumber { get; init; }
    public IImmutableList<RecipeData>? Recipes { get; init; }
}
