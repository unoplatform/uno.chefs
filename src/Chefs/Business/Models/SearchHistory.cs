namespace Chefs.Business.Models;


public record SearchHistory
{
	public IImmutableList<string> Searches { get; init; } = ImmutableList<string>.Empty;
}
