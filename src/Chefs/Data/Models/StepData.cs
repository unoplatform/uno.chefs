using System.Collections.Immutable;

namespace Chefs.Data;

public class StepData
{
    public int RecipeId { get; set; }
    public string? UrlVideo { get; set; }
    public int Number { get; set; }
    public TimeSpan CookTime { get; set; }
    public IImmutableList<string>? Cookware { get; set; }
    public IImmutableList<IngredientData>? Ingredients { get; set; }
    public string? Description { get; set; }
}
