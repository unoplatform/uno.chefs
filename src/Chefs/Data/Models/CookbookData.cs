using System.Collections.Immutable;

namespace Chefs.Data;

public class CookbookData
{
    public Guid Id { get; set; } 
    public int? UserId { get; set; }
    public string? Name { get; set; }
    public int PinsNumber { get; set; }
    public IImmutableList<RecipeData>? Recipes { get; set; }
}
