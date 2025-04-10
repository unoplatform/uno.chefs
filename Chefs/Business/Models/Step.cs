using Chefs.Services.Clients.Models;
using StepData = Chefs.Services.Clients.Models.StepData;

namespace Chefs.Business.Models;

public record Step
{
	public Step(StepData stepData)
	{
		Number = stepData.Number ?? 0;
		Name = stepData.Name;
		CookTime = ToTimeSpan(stepData.CookTime);
		Cookware = stepData.Cookware?.ToImmutableList() ?? ImmutableList<string>.Empty;
		Ingredients = stepData.Ingredients?.ToImmutableList() ?? ImmutableList<string>.Empty;
		Description = stepData.Description;
		UrlVideo = stepData.UrlVideo;
	}

	public int Number { get; init; }
	public string? Name { get; init; }
	public TimeSpan CookTime { get; init; }
	public IImmutableList<string>? Cookware { get; init; }
	public IImmutableList<string>? Ingredients { get; init; }
	public string? Description { get; init; }
	public string? UrlVideo { get; init; }

	internal StepData ToData() => new()
	{
		Number = Number,
		Name = Name,
		CookTime = new TimeSpanObject(),
		Cookware = Cookware?.ToList(),
		Ingredients = Ingredients?.ToList(),
		Description = Description,
		UrlVideo = UrlVideo
	};
	
	private static TimeSpan ToTimeSpan(TimeSpanObject? timeSpanObject)
	{
		return new TimeSpan(
			timeSpanObject?.Hours ?? 0,
			timeSpanObject?.Minutes ?? 0,
			timeSpanObject?.Seconds ?? 0);
	}
}
