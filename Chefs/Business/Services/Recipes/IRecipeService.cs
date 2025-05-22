namespace Chefs.Business.Services.Recipes;

/// <summary>
/// Implements recipe related methods
/// </summary>
public interface IRecipeService
{
	/// <summary>
	/// Retrieves all recipes from the API.
	/// </summary>
	/// <param name="ct">Cancellation token to cancel the operation.</param>
	/// <returns>A list of all recipes.</returns>
	ValueTask<IImmutableList<Recipe>> GetAll(CancellationToken ct);

	/// <summary>
	/// Adds a dislike to the specified review.
	/// </summary>
	/// <param name="review">The review to dislike.</param>
	/// <param name="ct">Cancellation token to cancel the operation.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	ValueTask DislikeReview(Review review, CancellationToken ct);

	/// <summary>
	/// Adds a like to the specified review.
	/// </summary>
	/// <param name="review">The review to like.</param>
	/// <param name="ct">Cancellation token to cancel the operation.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	ValueTask LikeReview(Review review, CancellationToken ct);

	/// <summary>
	/// Retrieves the count of recipes created by a specific user.
	/// </summary>
	/// <param name="userId">The ID of the user.</param>
	/// <param name="ct">Cancellation token to cancel the operation.</param>
	/// <returns>The count of recipes created by the user.</returns>
	ValueTask<int> GetCount(Guid userId, CancellationToken ct);

	/// <summary>
	/// Gets the list of favorited recipes.
	/// </summary>
	IListState<Recipe> FavoritedRecipes { get; }

	/// <summary>
	/// Retrieves recipes filtered by a specific category.
	/// </summary>
	/// <param name="categoryId">The ID of the category to filter by.</param>
	/// <param name="ct">Cancellation token to cancel the operation.</param>
	/// <returns>A list of recipes in the specified category.</returns>
	ValueTask<IImmutableList<Recipe>> GetByCategory(int categoryId, CancellationToken ct);

	/// <summary>
	/// Retrieves all categories from the API.
	/// </summary>
	/// <param name="ct">Cancellation token to cancel the operation.</param>
	/// <returns>A list of all categories.</returns>
	ValueTask<IImmutableList<Category>> GetCategories(CancellationToken ct);

	/// <summary>
	/// Retrieves all categories along with their recipe counts.
	/// </summary>
	/// <param name="ct">Cancellation token to cancel the operation.</param>
	/// <returns>A list of categories with their corresponding recipe counts.</returns>
	ValueTask<IImmutableList<CategoryWithCount>> GetCategoriesWithCount(CancellationToken ct);

	/// <summary>
	/// Retrieves trending recipes.
	/// </summary>
	/// <param name="ct">Cancellation token to cancel the operation.</param>
	/// <returns>A list of trending recipes.</returns>
	ValueTask<IImmutableList<Recipe>> GetTrending(CancellationToken ct);

	/// <summary>
	/// Retrieves popular recipes.
	/// </summary>
	/// <param name="ct">Cancellation token to cancel the operation.</param>
	/// <returns>A list of popular recipes.</returns>
	ValueTask<IImmutableList<Recipe>> GetPopular(CancellationToken ct);

	/// <summary>
	/// Retrieves recently added recipes.
	/// </summary>
	/// <param name="ct">Cancellation token to cancel the operation.</param>
	/// <returns>A list of recently added recipes.</returns>
	ValueTask<IImmutableList<Recipe>> GetRecent(CancellationToken ct);

	/// <summary>
	/// Searches for recipes based on a term and filter.
	/// </summary>
	/// <param name="term">The search term.</param>
	/// <param name="filter">The filter to apply to the search.</param>
	/// <param name="ct">Cancellation token to cancel the operation.</param>
	/// <returns>A list of recipes matching the search criteria.</returns>
	ValueTask<IImmutableList<Recipe>> Search(string term, SearchFilter filter, CancellationToken ct);

	/// <summary>
	/// Retrieves reviews for a specific recipe.
	/// </summary>
	/// <param name="recipeId">The ID of the recipe.</param>
	/// <param name="ct">Cancellation token to cancel the operation.</param>
	/// <returns>A list of reviews for the recipe.</returns>
	ValueTask<IImmutableList<Review>> GetReviews(Guid recipeId, CancellationToken ct);

	/// <summary>
	/// Retrieves ingredients for a specific recipe.
	/// </summary>
	/// <param name="recipeId">The ID of the recipe.</param>
	/// <param name="ct">Cancellation token to cancel the operation.</param>
	/// <returns>A list of ingredients for the recipe.</returns>
	ValueTask<IImmutableList<Ingredient>> GetIngredients(Guid recipeId, CancellationToken ct);

	/// <summary>
	/// Marks a recipe as a favorite.
	/// </summary>
	/// <param name="recipe">The recipe to mark as favorite.</param>
	/// <param name="ct">Cancellation token to cancel the operation.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	ValueTask Favorite(Recipe recipe, CancellationToken ct);

	/// <summary>
	/// Retrieves steps for a specific recipe.
	/// </summary>
	/// <param name="recipeId">The ID of the recipe.</param>
	/// <param name="ct">Cancellation token to cancel the operation.</param>
	/// <returns>A list of steps for the recipe.</returns>
	ValueTask<IImmutableList<Step>> GetSteps(Guid recipeId, CancellationToken ct);

	/// <summary>
	/// Retrieves recipes created by a specific user.
	/// </summary>
	/// <param name="userId">The ID of the user.</param>
	/// <param name="ct">Cancellation token to cancel the operation.</param>
	/// <returns>A list of recipes created by the user.</returns>
	ValueTask<IImmutableList<Recipe>> GetByUser(Guid userId, CancellationToken ct);

	/// <summary>
	/// Retrieves favorited recipes with pagination.
	/// </summary>
	/// <param name="pageSize">The number of items per page.</param>
	/// <param name="firstItemIndex">The index of the first item on the requested page.</param>
	/// <param name="ct">Cancellation token to cancel the operation.</param>
	/// <returns>A list of favorited recipes within the requested page.</returns>
	ValueTask<IImmutableList<Recipe>> GetFavoritedWithPagination(uint pageSize, uint firstItemIndex, CancellationToken ct);

	/// <summary>
	/// Retrieves recommended recipes.
	/// </summary>
	/// <param name="ct">Cancellation token to cancel the operation.</param>
	/// <returns>A list of recommended recipes.</returns>
	ValueTask<IImmutableList<Recipe>> GetRecommended(CancellationToken ct);

	/// <summary>
	/// Retrieves recipes from chefs.
	/// </summary>
	/// <param name="ct">Cancellation token to cancel the operation.</param>
	/// <returns>A list of recipes from chefs.</returns>
	ValueTask<IImmutableList<Recipe>> GetFromChefs(CancellationToken ct);

	/// <summary>
	/// Retrieves the search history.
	/// </summary>
	/// <returns>A list of search history terms.</returns>
	IImmutableList<string> GetSearchHistory();
}
