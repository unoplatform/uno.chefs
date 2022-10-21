﻿using System.Collections.Immutable;

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
    ValueTask<IImmutableList<Recipe>> GetByCategory(Category category, CancellationToken ct);

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
    /// Porpular creators related with the recipes
    /// </summary>
    /// <param name="ct"></param>
    /// <returns>
    /// Return users that they are popular by their recipes
    /// </returns>
    ValueTask<IImmutableList<User>> GetPopularCreators(CancellationToken ct);

    /// <summary>
    /// Cookbooks from api
    /// </summary>
    /// <param name="ct"></param>
    /// <returns>
    /// Get each cookbook from api that was saved
    /// </returns>
    ValueTask<IImmutableList<Cookbook>> GetCookbooks(CancellationToken ct);

    /// <summary>
    /// Filter recipes from api
    /// </summary>
    /// <param name="search">This is an object with attributes to filter recipes</param>
    /// <param name="ct"></param>
    /// <returns>
    /// Get recipes filter by different options selected by the user
    /// </returns>
    ValueTask<IImmutableList<Recipe>> Search(SearchFilter search, CancellationToken ct);
}
