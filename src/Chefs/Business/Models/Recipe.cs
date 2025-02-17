using Chefs.Services.Clients.Models;
using RecipeData = Chefs.Services.Clients.Models.RecipeData;

namespace Chefs.Business.Models;

public partial record Recipe : IChefEntity
{
	internal Recipe(RecipeData recipeData)
	{
		Id = recipeData.Id ?? Guid.Empty;
		UserId = recipeData.UserId ?? Guid.Empty;
		ImageUrl = recipeData.ImageUrl;
		Name = recipeData.Name;
		Serves = recipeData.Serves ?? 0;
		CookTime = recipeData.CookTime ?? new TimeSpanObject();
		Difficulty = (Difficulty)(recipeData.Difficulty ?? 0);
		Calories = recipeData.Calories;
		Details = recipeData.Details;
		Category = new Category(recipeData.Category);
		Date = recipeData.Date ?? DateTime.MinValue;
		IsFavorite = recipeData.IsFavorite ?? false;
		Nutrition = new Nutrition(recipeData?.Nutrition);
	}
	public Guid Id { get; init; }
	public Guid UserId { get; init; }
	public string? ImageUrl { get; init; }
	public string? Name { get; init; }
	public int Serves { get; init; }
	public TimeSpanObject CookTime { get; init; }
	public Difficulty Difficulty { get; init; }
	public string? Calories { get; init; }
	public string? Details { get; init; }
	public Category Category { get; init; }
	public DateTimeOffset Date { get; init; }
	public bool IsFavorite { get; init; }
	public Nutrition Nutrition { get; init; }

	//remove "kcal" unit from Calories property
	public string? CaloriesAmount => Calories?.Length > 4 ? Calories.Remove(Calories.Length - 4) : Calories;
	public string TimeCal
	{
		get
		{
			var timeSpan = ToTimeSpan(CookTime);
			return timeSpan > TimeSpan.FromHours(1)
				? $"{timeSpan:%h} hour {timeSpan:%m} mins • {Calories}"
				: $"{timeSpan:%m} mins • {Calories}";
		}
	}

	internal RecipeData ToData() => new()
	{
		Id = Id,
		UserId = UserId,
		ImageUrl = ImageUrl,
		Name = Name,
		Serves = Serves,
		CookTime = CookTime,
		Difficulty = (int)Difficulty,
		Calories = Calories,
		Details = Details,
		Category = Category.ToData(),
		Date = Date
	};
	private static TimeSpan ToTimeSpan(TimeSpanObject timeSpanObject)
	{
		return new TimeSpan(
			timeSpanObject?.Hours ?? 0,
			timeSpanObject?.Minutes ?? 0,
			timeSpanObject?.Seconds ?? 0);
	}
}
