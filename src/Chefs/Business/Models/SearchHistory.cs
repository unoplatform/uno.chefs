namespace Chefs.Business.Models;

public record SearchHistory
{
	public List<string> Searches { get; init; } = new();
}
