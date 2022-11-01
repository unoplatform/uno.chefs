using Chefs.Data;
using System.Collections.Immutable;

namespace Chefs.Business;

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
    /// Recipes with a specific category
    /// </summary>
    /// <param name="category">The specific category to filter recipes</param>
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
    /// Recipes in trending
    /// </summary>
    /// <param name="ct"></param>
    /// <returns>
    /// Get recipes filter in trending now
    /// </returns>
    ValueTask<IImmutableList<Recipe>> GetTrending(CancellationToken ct);

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
    /// <param name="search">This is an object with attributes to filter recipes</param>
    /// <param name="ct"></param>
    /// <returns>
    /// Get recipes filter by different options selected by the user
    /// </returns>
    ValueTask<IImmutableList<Recipe>> Search(string term, CancellationToken ct);

    /// <summary>
    /// Get recipe's reviews
    /// </summary>
    /// <param name="ct"></param>
    /// <returns>
    /// Recipe's reviews
    /// </returns>
    ValueTask<IImmutableList<Review>> GetReviews(Guid recipeId, CancellationToken ct);

    /// <summary>
    /// Save recipe 
    /// </summary>
    /// <param name="recipe"> recipe to save </param>
    /// <param name="ct"></param>
    /// <returns>
    /// </returns>
    ValueTask Save(Recipe recipe, CancellationToken ct);

    /// <summary>
    /// Create review for a recipe
    /// </summary>
    /// <param name="review"> review to create </param>
    /// <param name="ct"></param>
    /// <returns>
    /// </returns>
    ValueTask CreateReview(Guid recipeId, string review, CancellationToken ct);

    /// <summary>
    /// Get review's steps
    /// </summary>
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
}
