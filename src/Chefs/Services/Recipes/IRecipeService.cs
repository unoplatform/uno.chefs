namespace Chefs.Services.Recipes;

/// <summary>
/// Implements recipe related methods
/// </summary>
public interface IRecipeService
{
	/// <summary>
	/// Recipes method
	/// </summary>
	/// <param name="ct"></param>
	/// <returns>
	/// Get each recipe from api
	/// </returns>
	ValueTask<IImmutableList<Recipe>> GetAll(CancellationToken ct);

	/// <summary>
	/// Add current user dislike recipe review
	/// </summary>
	/// <param name="review">review to update</param>
	/// <param name="ct"></param>
	/// <returns>
	/// </returns>
	ValueTask DislikeReview(Review review, CancellationToken ct);

	/// <summary>
	/// Add current user like recipe review
	/// </summary>
	/// <param name="review">review to update</param>
	/// <param name="ct"></param>
	/// <returns>
	/// </returns>
	ValueTask LikeReview(Review review, CancellationToken ct);

	/// <summary>
	/// Recipes method
	/// </summary>
	/// <param name="ct"></param>
	/// <returns>
	/// Get recipes count by user
	/// </returns>
	ValueTask<int> GetCount(Guid userId, CancellationToken ct);

	/// <summary>
	/// Favorited recipes.
	/// </summary>
	IListState<Recipe> FavoritedRecipes { get; }

	/// <summary>
	/// Recipes with a specific category
	/// </summary>
	/// <param name="categoryId">The specific category to filter recipes</param>
	/// <param name="ct"></param>
	/// <returns>
	/// Get each recipe from api filter by a category
	/// </returns>
	ValueTask<IImmutableList<Recipe>> GetByCategory(int categoryId, CancellationToken ct);

	/// <summary>
	/// Categories from api
	/// </summary>
	/// <param name="ct"></param>
	/// <returns>
	/// Get each category from api
	/// </returns>
	ValueTask<IImmutableList<Category>> GetCategories(CancellationToken ct);

	/// <summary>
	/// Categories from api with count
	/// </summary>
	/// <param name="ct"></param>
	/// <returns>
	/// Get each category from api with their corresponding count
	/// </returns>
	ValueTask<IImmutableList<CategoryWithCount>> GetCategoriesWithCount(CancellationToken ct);

	/// <summary>
	/// Recipes in trending
	/// </summary>
	/// <param name="ct"></param>
	/// <returns>
	/// Get recipes filter in trending now
	/// </returns>
	ValueTask<IImmutableList<Recipe>> GetTrending(CancellationToken ct);

	/// <summary>
	/// Popular Recipes
	/// </summary>
	/// <param name="ct"></param>
	/// <returns>
	/// Get popular recipes filter 
	/// </returns>
	ValueTask<IImmutableList<Recipe>> GetPopular(CancellationToken ct);

	/// <summary>
	/// Recipes recently added
	/// </summary>
	/// <param name="ct"></param>
	/// <returns>
	/// Get recent recipes or new recipes
	/// </returns>
	ValueTask<IImmutableList<Recipe>> GetRecent(CancellationToken ct);

	/// <summary>
	/// Filter recipes from api
	/// </summary>
	/// <param name="term">The search term</param>
	/// <param name="ct"></param>
	/// <returns>
	/// Get recipes filter by different options selected by the user
	/// </returns>
	ValueTask<IImmutableList<Recipe>> Search(string term, SearchFilter filter, CancellationToken ct);

	/// <summary>
	/// Get recipe's reviews
	/// </summary>
	/// <param name="recipeId">id from the recipe</param>
	/// <param name="ct"></param>
	/// <returns>
	/// Recipe's reviews
	/// </returns>
	ValueTask<IImmutableList<Review>> GetReviews(Guid recipeId, CancellationToken ct);

	/// <summary>
	/// Get recipe's ingredients
	/// </summary>
	/// <param name="recipeId">id from the recipe</param>
	/// <param name="ct">Recipe Guid</param>
	/// <returns>
	/// Recipe's ingredients
	/// </returns>
	ValueTask<IImmutableList<Ingredient>> GetIngredients(Guid recipeId, CancellationToken ct);

	/// <summary>
	/// Save recipe 
	/// </summary>
	/// <param name="recipe"> recipe to save </param>
	/// <param name="ct"></param>
	/// <returns>
	/// </returns>
	ValueTask Favorite(Recipe recipe, CancellationToken ct);

	/// <summary>
	/// Create review for a recipe
	/// </summary>
	/// <param name="review"> review to create </param>
	/// <param name="ct"></param>
	/// <returns>
	/// </returns>
	ValueTask<Review> CreateReview(Guid recipeId, string review, CancellationToken ct);

	/// <summary>
	/// Get review's steps
	/// </summary>
	/// <param name="recipeId">id from the recipe</param>
	/// <param name="ct"></param>
	/// <returns>
	/// Recipe's steps
	/// </returns>
	ValueTask<IImmutableList<Step>> GetSteps(Guid recipeId, CancellationToken ct);

	/// <summary>
	/// Recipes by user
	/// </summary>
	/// <param name="ct"></param>
	/// <returns>
	/// User's recipes
	/// </returns>
	ValueTask<IImmutableList<Recipe>> GetByUser(Guid userId, CancellationToken ct);


	/// <summary>
	/// Recipes favorited by the current user
	/// </summary>
	/// <param name="ct"></param>
	/// <returns>
	/// Current user's recipes
	/// </returns>
	ValueTask<IImmutableList<Recipe>> GetFavorited(CancellationToken ct);

	/// <summary>
	/// Recipes favorited by the current user, supports pagination
	/// </summary>
	/// <param name="pageSize">number of items to display per page</param>
	/// <param name="firstItemIndex">index of the first item on the requested page</param>
	/// <param name="ct"></param>
	/// <returns>
	/// Current user's recipes within the requested page
	/// </returns>
	ValueTask<IImmutableList<Recipe>> GetFavoritedWithPagination(uint pageSize, uint firstItemIndex, CancellationToken ct);

	ValueTask<IImmutableList<Recipe>> GetRecommended(CancellationToken ct);

	ValueTask<IImmutableList<Recipe>> GetFromChefs(CancellationToken ct);

	IImmutableList<string> GetSearchHistory();
}
