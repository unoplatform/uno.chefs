using Chefs.Services.Clients.Models;

namespace Chefs.Business.Models;

public record SearchFilter(
	FilterGroup? FilterGroup = null,
	Data.Time? Time = null,
	Difficulty? Difficulty = null,
	int? Serves = null,
	Category? Category = null)
{
	public bool HasFilter => FilterGroup != null || Time != null ||
	                         Difficulty != null || Category != null || Serves != null;
	
	public bool Match(Recipe recipe)
	{
		var maxTime = Time switch
		{
			Data.Time.Under15min => TimeSpan.FromMinutes(15),
			Data.Time.Under30min => TimeSpan.FromMinutes(30),
			Data.Time.Under60min => TimeSpan.FromMinutes(60),
			_ => TimeSpan.MaxValue,
		};
		
		var cookTimeSpan = ToTimeSpan(recipe.CookTime);
		
		return (Difficulty == null || recipe.Difficulty == Difficulty) &&
		       (Time == null || cookTimeSpan < maxTime) &&
		       (Category == null || recipe.Category.Id == Category.Id || recipe.Category.Name == Category.Name) &&
		       (Serves == null || Serves == recipe.Serves);
	}
	private static TimeSpan ToTimeSpan(TimeSpanObject timeSpanObject)
	{
		return new TimeSpan(timeSpanObject?.Ticks ?? 0);
	}
}
