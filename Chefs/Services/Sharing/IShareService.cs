namespace Chefs.Services.Sharing;

/// <summary>
/// Provides methods for sharing recipe content using native sharing features.
/// </summary>
public interface IShareService
{
	/// <summary>
	/// Opens the native sharing interface for a recipe and its steps.
	/// </summary>
	/// <param name="recipe">The recipe to share.</param>
	/// <param name="steps">The steps of the recipe to share.</param>
	/// <param name="ct">A cancellation token.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	Task ShareRecipe(Recipe recipe, IImmutableList<Step> steps, CancellationToken ct);
}
