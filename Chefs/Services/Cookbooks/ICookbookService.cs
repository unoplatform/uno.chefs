namespace Chefs.Services.Cookbooks;

/// <summary>
/// Provides methods for creating, updating, saving, and retrieving cookbooks.
/// </summary>
public interface ICookbookService
{
	/// <summary>
	/// Creates a new cookbook with the specified name and recipes.
	/// </summary>
	/// <param name="name">The name of the cookbook.</param>
	/// <param name="recipes">The list of recipes to include in the cookbook.</param>
	/// <param name="ct">A cancellation token.</param>
	/// <returns>The created <see cref="Cookbook"/>.</returns>
	ValueTask<Cookbook> Create(string name, IImmutableList<Recipe> recipes, CancellationToken ct);

	/// <summary>
	/// Updates an existing cookbook with a new list of recipes.
	/// </summary>
	/// <param name="cookbook">The cookbook to update.</param>
	/// <param name="recipes">The updated list of recipes.</param>
	/// <param name="ct">A cancellation token.</param>
	/// <returns>The updated <see cref="Cookbook"/>.</returns>
	ValueTask<Cookbook> Update(Cookbook cookbook, IImmutableList<Recipe> recipes, CancellationToken ct);

	/// <summary>
	/// Updates the details of an existing cookbook.
	/// </summary>
	/// <param name="cookbook">The cookbook to update.</param>
	/// <param name="ct">A cancellation token.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	ValueTask Update(Cookbook cookbook, CancellationToken ct);

	/// <summary>
	/// Saves the specified cookbook.
	/// </summary>
	/// <param name="cookbook">The cookbook to save.</param>
	/// <param name="ct">A cancellation token.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	ValueTask Save(Cookbook cookbook, CancellationToken ct);

	/// <summary>
	/// Retrieves the cookbooks saved by the current user.
	/// </summary>
	/// <param name="ct">A cancellation token.</param>
	/// <returns>A list of saved <see cref="Cookbook"/> objects.</returns>
	ValueTask<IImmutableList<Cookbook>> GetSaved(CancellationToken ct);

	/// <summary>
	/// Retrieves the cookbooks created by the specified user.
	/// </summary>
	/// <param name="userId">The ID of the user whose cookbooks to retrieve.</param>
	/// <param name="ct">A cancellation token.</param>
	/// <returns>A list of <see cref="Cookbook"/> objects created by the user.</returns>
	ValueTask<IImmutableList<Cookbook>> GetByUser(Guid userId, CancellationToken ct);
}
