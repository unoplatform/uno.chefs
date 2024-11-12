using LiveChartsCore;
using LiveChartsCore.Kernel.Sketches;

namespace Chefs.Business.Models;

public partial record Recipe : IChefEntity
{
	internal Recipe(RecipeData recipeData)
	{
		Id = recipeData.Id;
		UserId = recipeData.UserId;
		ImageUrl = recipeData.ImageUrl;
		Name = recipeData.Name;
		Serves = recipeData.Serves;
		CookTime = recipeData.CookTime;
		Difficulty = recipeData.Difficulty;
		Calories = recipeData.Calories;
		Details = recipeData.Details;
		Category = new Category(recipeData.Category);
		Date = recipeData.Date;
		IsFavorite = recipeData.IsFavorite;
		Nutrition = new Nutrition(recipeData?.Nutrition);
	}
	public Guid Id { get; init; }
	public Guid UserId { get; init; }
	public string? ImageUrl { get; init; }
	public string? Name { get; init; }
	public int Serves { get; init; }
	public TimeSpan CookTime { get; init; }
	public Difficulty Difficulty { get; init; }
	public string? Calories { get; init; }
	public string? Details { get; init; }
	public Category Category { get; init; }
	public DateTime Date { get; init; }
	public bool IsFavorite { get; init; }
	public Nutrition Nutrition { get; init; }

	//remove "kcal" unit from Calories property
	public string? CaloriesAmount => Calories?.Remove(Calories.Length - 4);

	public string TimeCal => CookTime > TimeSpan.FromHours(1) ?
		String.Format("{0:%h} hour {0:%m} mins • {1}", CookTime, Calories) :
		String.Format("{0:%m} mins • {1}", CookTime, Calories);

	internal RecipeData ToData() => new()
	{
		Id = Id,
		UserId = UserId,
		ImageUrl = ImageUrl,
		Name = Name,
		Serves = Serves,
		CookTime = CookTime,
		Difficulty = Difficulty,
		Calories = Calories,
		Details = Details,
		Category = Category.ToData(),
		Date = Date
	};

	public IReadOnlyCollection<ISeries> Series { get; set; } = [];
	public IReadOnlyCollection<ICartesianAxis> XAxes { get; set; } = [];
	public IReadOnlyCollection<ICartesianAxis> YAxes { get; set; } = [];
}
