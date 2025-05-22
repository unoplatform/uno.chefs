namespace Chefs.Business.Services.Cookbooks;

public interface ICookbookService
{
	/// <summary>
	/// Add cookbook created by the user.
	/// </summary>
	/// <param name="name">The name of the cookbook to add.</param>
	/// <param name="recipes">The list of recipes to include in the cookbook.</param>
	/// <param name="ct">The cancellation token to observe while waiting for the task to complete.</param>
	/// <returns>The created cookbook.</returns>
	ValueTask<Cookbook> Create(string name, IImmutableList<Recipe> recipes, CancellationToken ct);

	/// <summary>
	/// Update an existing cookbook with new details and recipes.
	/// </summary>
	/// <param name="cookbook">The cookbook to update.</param>
	/// <param name="recipes">The updated list of recipes for the cookbook.</param>
	/// <param name="ct">The cancellation token to observe while waiting for the task to complete.</param>
	/// <returns>The updated cookbook.</returns>
	ValueTask<Cookbook> Update(Cookbook cookbook, IImmutableList<Recipe> recipes, CancellationToken ct);

	/// <summary>
	/// Update an existing cookbook with new details.
	/// </summary>
	/// <param name="cookbook">The cookbook to update.</param>
	/// <param name="ct">The cancellation token to observe while waiting for the task to complete.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	ValueTask Update(Cookbook cookbook, CancellationToken ct);

	/// <summary>
	/// Save a cookbook that the user wants to persist.
	/// </summary>
	/// <param name="cookbook">The cookbook to save.</param>
	/// <param name="ct">The cancellation token to observe while waiting for the task to complete.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	ValueTask Save(Cookbook cookbook, CancellationToken ct);

	/// <summary>
	/// Retrieve all cookbooks saved from the API.
	/// </summary>
	/// <param name="ct">The cancellation token to observe while waiting for the task to complete.</param>
	/// <returns>A list of saved cookbooks.</returns>
	ValueTask<IImmutableList<Cookbook>> GetSaved(CancellationToken ct);

	/// <summary>
	/// Retrieve all cookbooks created by a specific user.
	/// </summary>
	/// <param name="userId">The ID of the user whose cookbooks are to be retrieved.</param>
	/// <param name="ct">The cancellation token to observe while waiting for the task to complete.</param>
	/// <returns>A list of the user's cookbooks.</returns>
	ValueTask<IImmutableList<Cookbook>> GetByUser(Guid userId, CancellationToken ct);
}
