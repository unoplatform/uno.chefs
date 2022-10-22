using System.Collections.Immutable;
using Chefs.Business;

namespace Chefs.Data;

public class RecipeData
{
    public Guid? Id { get; set; }
    public int? UserId { get; set; }
    public IImmutableList<StepData>? Steps { get; set; }
    public string? ImageUrl { get; set; }
    public string? Name { get; set; }
    public int Serves { get; init; }
    public TimeSpan CookTime { get; set; }
    public Difficulties Difficulty { get; set; }
    public IImmutableList<IngredientData>? Ingredients { get; set; }
    public string? Calories { get; set; }
    public IImmutableList<Review>? Reviews { get; set; }
    public string? Details { get; set; }
    public CategoryData? Category { get; set; }
    public DateTime Date { get; set; }
}
