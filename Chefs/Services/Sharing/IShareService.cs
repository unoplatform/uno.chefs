namespace Chefs.Services.Sharing;

/// <summary>
/// Implements content sharing related methods
/// </summary>
public interface IShareService
{
	Task ShareRecipe(Recipe recipe, IImmutableList<Step> steps, CancellationToken ct);	
}
