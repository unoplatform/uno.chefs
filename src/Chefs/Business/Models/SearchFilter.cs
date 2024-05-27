namespace Chefs.Business.Models;


public record SearchFilter(

	OrganizeCategory? OrganizeCategory = null,
	Time? Time = null,
	Difficulty? Difficulty = null,
	int? Serves = null,
	Category? Category = null)

	FilterGroup? FilterGroup,
	Time? Time,
	Difficulty? Difficulty,
	int? Serves,
	Category? Category)
{
	public bool HasFilter => RecipeCategoryType != null || Time != null ||
	public bool HasFilter => FilterGroup != null || Time != null ||
		Difficulty != null || Category != null || Serves != null;

	public bool Match(Recipe recipe)
	{
		var time = Time switch
		{
			Data.Time.Under15min => new TimeSpan(0, 15, 0),
			Data.Time.Under30min => new TimeSpan(0, 30, 0),
			Data.Time.Under60min => new TimeSpan(0, 60, 0),

			_ => TimeSpan.Zero,
		};

		return (Difficulty == null || recipe.Difficulty == Difficulty) &&
			   (Time == null || recipe.CookTime < time) &&
			   (Category == null || recipe.Category.Id == Category.Id || recipe.Category.Name == Category.Name) &&
			   (Serves == null || Serves == recipe.Serves);
	}
}
