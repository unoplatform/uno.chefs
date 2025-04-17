using System.Text.Json.Serialization;
using Chefs.Converters;

namespace Chefs.Data;

public class RecipeData
{
	public Guid Id { get; set; }
	public Guid UserId { get; set; }
	public List<StepData>? Steps { get; set; }
	public string? ImageUrl { get; set; }
	public string? Name { get; set; }
	public int Serves { get; set; }

	[JsonConverter(typeof(TimeSpanObjectConverter))]
	public TimeSpan CookTime { get; set; }
	public Difficulty Difficulty { get; set; }
	public List<IngredientData>? Ingredients { get; set; }
	public string? Calories { get; set; }
	public List<ReviewData>? Reviews { get; set; }
	public string? Details { get; set; }
	public CategoryData? Category { get; set; }
	public DateTime Date { get; set; }
	public bool IsFavorite { get; set; }
	public NutritionData? Nutrition { get; set; } = new(30, 101, 30, 110, 300, 75);
}
