using System.Collections.Immutable;
using Chefs.Business;

namespace Chefs.Data;

public record RecipeData
{
    public int? Id { get; init; }
    public int? UserId { get; init; }
    public IImmutableList<StepData>? Steps { get; init; }
    public string? ImageUrl { get; init; }
    public string? Name { get; init; }
    public int Serves { get; init; }
    public TimeSpan CookTime { get; init; }
    public Difficulties Difficulty { get; init; }
    public IImmutableList<IngredientData>? Ingredients { get; init; }
    public string? Calories { get; init; }
    public IImmutableList<Review>? Reviews { get; init; }
    public string? Details { get; init; }
    public CategoryData? Category { get; init; }
    public DateTime Date { get; init; }
    public bool Save { get; init; }
}
