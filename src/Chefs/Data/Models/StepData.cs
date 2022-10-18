using System.Collections.Immutable;

namespace Chefs.Data;

public record StepData
{
    public int RecipeId { get; init; }
    public string? UrlVideo { get; init; }
    public int Number { get; init; }
    public TimeSpan CookTime { get; init; }
    public IImmutableList<string>? Cookware { get; init; }
    public IImmutableList<IngredientData>? Ingredients { get; init; }
    public string? Description { get; init; }
}
