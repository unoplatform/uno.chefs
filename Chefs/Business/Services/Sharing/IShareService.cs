namespace Chefs.Business.Services.Sharing;

/// <summary>
/// Implements content sharing related methods
/// </summary>
public interface IShareService
{
	/// <summary>
	/// Open native sharing for a recipe and its steps
	/// </summary>
	/// <param name="recipe">Recipe to share</param>
	/// <param name="steps">Recipe's steps</param>
	/// <param name="ct">Cancellation token</param>
	void ShareRecipe(Recipe recipe, IImmutableList<Step> steps, CancellationToken ct);
}
