namespace Chefs.Services.Sharing;

/// <summary>
/// Implements content sharing related methods
/// </summary>
public interface IShareService
{
	///<summary>
	/// Open native sharing for a recipe and its steps
	/// </summary>
	/// <param name="recipe">recipe to share</param>
	/// <param name="steps">recipe's steps</param>
	/// <param name="ct"></param>
	/// <returns>
	/// </returns>
	Task ShareRecipe(Recipe recipe, IImmutableList<Step> steps, CancellationToken ct);	
}
