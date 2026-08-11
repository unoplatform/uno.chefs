using Chefs.Business.Services.Users;
using Chefs.Client;
using ReviewData = Chefs.Client.Models.ReviewData;

namespace Chefs.Business.Services.Recipes;

public class RecipeService(
	ChefsApiClient api,
	IUserService userService,
	IWritableOptions<SearchHistory> searchOptions,
	IMessenger messenger,
	ILogger<RecipeService> logger)
	: IRecipeService
{
	private int _lastTextLength;

	public async ValueTask<IImmutableList<Recipe>> GetAll(CancellationToken ct)
	{
		try
		{
			return await GetAllCore(ct);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Failed to get all recipes");
			throw;
		}
	}

	private async ValueTask<IImmutableList<Recipe>> GetAllCore(CancellationToken ct)
	{
		var recipesData = await api.Api.Recipe.GetAsync(cancellationToken: ct);
		return recipesData?.Select(r => new Recipe(r)).ToImmutableList() ?? ImmutableList<Recipe>.Empty;
	}

	public async ValueTask<int> GetCount(Guid userId, CancellationToken ct)
	{
		try
		{
			return await GetCountCore(userId, ct);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Failed to get recipe count for user {UserId}", userId);
			throw;
		}
	}

	private async ValueTask<int> GetCountCore(Guid userId, CancellationToken ct)
	{
		var countData = await api.Api.Recipe.Count.GetAsync(q => q.QueryParameters.UserId = userId, cancellationToken: ct);
		return (int)countData;
	}

	public async ValueTask<IImmutableList<Recipe>> GetByCategory(int categoryId, CancellationToken ct)
	{
		try
		{
			return await GetByCategoryCore(categoryId, ct);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Failed to get recipes by category {CategoryId}", categoryId);
			throw;
		}
	}

	private async ValueTask<IImmutableList<Recipe>> GetByCategoryCore(int categoryId, CancellationToken ct)
	{
		var recipesData = await api.Api.Recipe.GetAsync(cancellationToken: ct);
		return recipesData?.Where(r => r.Category?.Id == categoryId).Select(r => new Recipe(r)).ToImmutableList() ?? ImmutableList<Recipe>.Empty;
	}

	public async ValueTask<IImmutableList<Category>> GetCategories(CancellationToken ct)
	{
		try
		{
			return await GetCategoriesCore(ct);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Failed to get categories");
			throw;
		}
	}

	private async ValueTask<IImmutableList<Category>> GetCategoriesCore(CancellationToken ct)
	{
		var categoriesData = await api.Api.Recipe.Categories.GetAsync(cancellationToken: ct);
		return categoriesData?.Select(c => new Category(c)).ToImmutableList() ?? ImmutableList<Category>.Empty;
	}

	public async ValueTask<IImmutableList<CategoryWithCount>> GetCategoriesWithCount(CancellationToken ct)
	{
		try
		{
			return await GetCategoriesWithCountCore(ct);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Failed to get categories with count");
			throw;
		}
	}

	private async ValueTask<IImmutableList<CategoryWithCount>> GetCategoriesWithCountCore(CancellationToken ct)
	{
		var categories = await GetCategoriesCore(ct);
		var tasks = categories.Select(async category =>
		{
			var recipesByCategory = await GetByCategoryCore(category.Id ?? 0, ct);
			return new CategoryWithCount(recipesByCategory.Count, category);
		});

		var categoriesWithCount = await Task.WhenAll(tasks);
		return categoriesWithCount.ToImmutableList();
	}

	public async ValueTask<IImmutableList<Recipe>> GetRecent(CancellationToken ct)
	{
		try
		{
			return await GetRecentCore(ct);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Failed to get recent recipes");
			throw;
		}
	}

	private async ValueTask<IImmutableList<Recipe>> GetRecentCore(CancellationToken ct)
	{
		var recipesData = await api.Api.Recipe.GetAsync(cancellationToken: ct);
		return recipesData?.Select(r => new Recipe(r)).OrderByDescending(x => x.Date).Take(7).ToImmutableList() ?? ImmutableList<Recipe>.Empty;
	}

	public async ValueTask<IImmutableList<Recipe>> GetTrending(CancellationToken ct)
	{
		try
		{
			return await GetTrendingCore(ct);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Failed to get trending recipes");
			throw;
		}
	}

	private async ValueTask<IImmutableList<Recipe>> GetTrendingCore(CancellationToken ct)
	{
		var trendingRecipesData = await api.Api.Recipe.Trending.GetAsync(cancellationToken: ct);
		return trendingRecipesData?.Select(r => new Recipe(r)).ToImmutableList() ?? ImmutableList<Recipe>.Empty;
	}

	public async ValueTask<IImmutableList<Recipe>> GetPopular(CancellationToken ct)
	{
		try
		{
			return await GetPopularCore(ct);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Failed to get popular recipes");
			throw;
		}
	}

	private async ValueTask<IImmutableList<Recipe>> GetPopularCore(CancellationToken ct)
	{
		var popularRecipesData = await api.Api.Recipe.Popular.GetAsync(cancellationToken: ct);
		return popularRecipesData?.Select(r => new Recipe(r)).ToImmutableList() ?? ImmutableList<Recipe>.Empty;
	}

	public async ValueTask<IImmutableList<Recipe>> Search(string term, SearchFilter filter, CancellationToken ct)
	{
		try
		{
			return await SearchCore(term, filter, ct);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Failed to search recipes with term {Term}", term);
			throw;
		}
	}

	private async ValueTask<IImmutableList<Recipe>> SearchCore(string term, SearchFilter filter, CancellationToken ct)
	{
		var recipesToSearch = filter.FilterGroup switch
		{
			FilterGroup.Popular => await GetPopularCore(ct),
			FilterGroup.Trending => await GetTrendingCore(ct),
			FilterGroup.Recent => await GetRecentCore(ct),
			_ => await GetAllCore(ct)
		};

		if (string.IsNullOrWhiteSpace(term))
		{
			_lastTextLength = 0;
			return recipesToSearch;
		}
		else
		{
			await SaveSearchHistory(term);
			return GetRecipesByText(recipesToSearch, term);
		}
	}

	public IImmutableList<string> GetSearchHistory()
		=> searchOptions.Value.Searches.Take(3).ToImmutableList();

	public async ValueTask<IImmutableList<Review>> GetReviews(Guid recipeId, CancellationToken ct)
	{
		try
		{
			return await GetReviewsCore(recipeId, ct);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Failed to get reviews for recipe {RecipeId}", recipeId);
			throw;
		}
	}

	private async ValueTask<IImmutableList<Review>> GetReviewsCore(Guid recipeId, CancellationToken ct)
	{
		var reviewsData = await api.Api.Recipe[recipeId].Reviews.GetAsync(cancellationToken: ct);
		return reviewsData?.Select(x => new Review(x)).ToImmutableList() ?? ImmutableList<Review>.Empty;
	}

	public async ValueTask<IImmutableList<Step>> GetSteps(Guid recipeId, CancellationToken ct)
	{
		try
		{
			return await GetStepsCore(recipeId, ct);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Failed to get steps for recipe {RecipeId}", recipeId);
			throw;
		}
	}

	private async ValueTask<IImmutableList<Step>> GetStepsCore(Guid recipeId, CancellationToken ct)
	{
		var stepsData = await api.Api.Recipe[recipeId].Steps.GetAsync(cancellationToken: ct);
		return stepsData?.Select(x => new Step(x)).ToImmutableList() ?? ImmutableList<Step>.Empty;
	}

	public async ValueTask<IImmutableList<Ingredient>> GetIngredients(Guid recipeId, CancellationToken ct)
	{
		try
		{
			return await GetIngredientsCore(recipeId, ct);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Failed to get ingredients for recipe {RecipeId}", recipeId);
			throw;
		}
	}

	private async ValueTask<IImmutableList<Ingredient>> GetIngredientsCore(Guid recipeId, CancellationToken ct)
	{
		var ingredientsData = await api.Api.Recipe[recipeId].Ingredients.GetAsync(cancellationToken: ct);
		return ingredientsData?.Select(x => new Ingredient(x)).ToImmutableList() ?? ImmutableList<Ingredient>.Empty;
	}

	public async ValueTask<IImmutableList<Recipe>> GetByUser(Guid userId, CancellationToken ct)
	{
		try
		{
			return await GetByUserCore(userId, ct);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Failed to get recipes by user {UserId}", userId);
			throw;
		}
	}

	private async ValueTask<IImmutableList<Recipe>> GetByUserCore(Guid userId, CancellationToken ct)
	{
		var recipesData = await api.Api.Recipe.GetAsync(cancellationToken: ct);
		return recipesData?.Where(r => r.UserId == userId).Select(x => new Recipe(x)).ToImmutableList() ?? ImmutableList<Recipe>.Empty;
	}

	public async ValueTask<Review> CreateReview(Guid recipeId, string review, CancellationToken ct)
	{
		try
		{
			return await CreateReviewCore(recipeId, review, ct);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Failed to create review for recipe {RecipeId}", recipeId);
			throw;
		}
	}

	private async ValueTask<Review> CreateReviewCore(Guid recipeId, string review, CancellationToken ct)
	{
		var reviewData = new ReviewData { RecipeId = recipeId, Description = review };
		var createdReviewData = await api.Api.Recipe.Review.PostAsync(reviewData, cancellationToken: ct);
		return new Review(createdReviewData);
	}

	public IListState<Recipe> FavoritedRecipes => ListState<Recipe>.Async(this, GetFavorited);

	public async ValueTask<IImmutableList<Recipe>> GetFavoritedWithPagination(uint pageSize, uint firstItemIndex, CancellationToken ct)
	{
		try
		{
			return await GetFavoritedWithPaginationCore(pageSize, firstItemIndex, ct);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Failed to get favorited recipes with pagination");
			throw;
		}
	}

	private async ValueTask<IImmutableList<Recipe>> GetFavoritedWithPaginationCore(uint pageSize, uint firstItemIndex, CancellationToken ct)
	{
		var favoritedRecipes = await GetFavorited(ct);
		return favoritedRecipes
			.Skip((int)firstItemIndex)
			.Take((int)pageSize)
			.ToImmutableList();
	}

	public async ValueTask Favorite(Recipe recipe, CancellationToken ct)
	{
		try
		{
			await FavoriteCore(recipe, ct);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Failed to favorite recipe {RecipeId}", recipe.Id);
			throw;
		}
	}

	private async ValueTask FavoriteCore(Recipe recipe, CancellationToken ct)
	{
		var currentUser = await userService.GetCurrent(ct);
		var updatedRecipe = recipe with { IsFavorite = !recipe.IsFavorite };
		await api.Api.Recipe.Favorited.PostAsync(q =>
		{
			q.QueryParameters.RecipeId = updatedRecipe.Id;
			q.QueryParameters.UserId = currentUser.Id;
		}, cancellationToken: ct);

		if (updatedRecipe.IsFavorite)
		{
			await FavoritedRecipes.AddAsync(updatedRecipe, ct: ct);
		}
		else
		{
			await FavoritedRecipes.RemoveAllAsync(r => r.Id == updatedRecipe.Id, ct: ct);
		}

		messenger.Send(new EntityMessage<Recipe>(EntityChange.Updated, updatedRecipe));
	}

	public async ValueTask LikeReview(Review review, CancellationToken ct)
	{
		try
		{
			await LikeReviewCore(review, ct);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Failed to like review {ReviewId}", review.Id);
			throw;
		}
	}

	private async ValueTask LikeReviewCore(Review review, CancellationToken ct)
	{
		var reviewData = review.ToData();
		var updatedReviewData = await api.Api.Recipe.Review.Like.PostAsync(reviewData, cancellationToken: ct);
		var updatedReview = new Review(updatedReviewData);
		messenger.Send(new EntityMessage<Review>(EntityChange.Updated, updatedReview));
	}

	public async ValueTask DislikeReview(Review review, CancellationToken ct)
	{
		try
		{
			await DislikeReviewCore(review, ct);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Failed to dislike review {ReviewId}", review.Id);
			throw;
		}
	}

	private async ValueTask DislikeReviewCore(Review review, CancellationToken ct)
	{
		var reviewData = review.ToData();
		var updatedReviewData = await api.Api.Recipe.Review.Dislike.PostAsync(reviewData, cancellationToken: ct);
		var updatedReview = new Review(updatedReviewData);
		messenger.Send(new EntityMessage<Review>(EntityChange.Updated, updatedReview));
	}

	public async ValueTask<IImmutableList<Recipe>> GetRecommended(CancellationToken ct)
	{
		try
		{
			return await GetRecommendedCore(ct);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Failed to get recommended recipes");
			throw;
		}
	}

	private async ValueTask<IImmutableList<Recipe>> GetRecommendedCore(CancellationToken ct)
	{
		var recipesData = await api.Api.Recipe.GetAsync(cancellationToken: ct);
		return recipesData?.Select(r => new Recipe(r)).OrderBy(_ => Guid.NewGuid()).Take(4).ToImmutableList() ?? ImmutableList<Recipe>.Empty;
	}

	public async ValueTask<IImmutableList<Recipe>> GetFromChefs(CancellationToken ct)
	{
		try
		{
			return await GetFromChefsCore(ct);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Failed to get recipes from chefs");
			throw;
		}
	}

	private async ValueTask<IImmutableList<Recipe>> GetFromChefsCore(CancellationToken ct)
	{
		var recipesData = await api.Api.Recipe.GetAsync(cancellationToken: ct);
		return recipesData?.Select(r => new Recipe(r)).OrderBy(_ => Guid.NewGuid()).Take(4).ToImmutableList() ?? ImmutableList<Recipe>.Empty;
	}

	private async ValueTask<IImmutableList<Recipe>> GetFavorited(CancellationToken ct)
	{
		var currentUser = await userService.GetCurrent(ct);
		var favoritedRecipesData = await api.Api.Recipe.Favorited.GetAsync(config => config.QueryParameters.UserId = currentUser.Id, cancellationToken: ct);
		return favoritedRecipesData?.Select(r => new Recipe(r)).ToImmutableList() ?? ImmutableList<Recipe>.Empty;
	}

	private async Task SaveSearchHistory(string text)
	{
		if (_lastTextLength <= text.Length) _lastTextLength = text.Length;

		var searchHistory = searchOptions.Value.Searches;
		if (!string.IsNullOrWhiteSpace(text))
		{
			if (searchHistory.Count == 0 || _lastTextLength == 1)
			{
				await searchOptions.UpdateAsync(h => h with { Searches = searchHistory.Prepend(text).ToList() });
			}
			else if (searchHistory.FirstOrDefault() is { } latestTerm
					 && (text.Contains(latestTerm) || latestTerm.Contains(text))
					 && _lastTextLength == text.Length)
			{
				await searchOptions.UpdateAsync(h => h with
				{
					Searches = searchHistory.Skip(1).Prepend(text).ToList(),
				});
			}
		}
	}

	private IImmutableList<Recipe> GetRecipesByText(IEnumerable<Recipe> recipes, string text)
		=> recipes
			.Where(r => r.Name?.Contains(text, StringComparison.OrdinalIgnoreCase) == true
						|| r.Category?.Name?.Contains(text, StringComparison.OrdinalIgnoreCase) == true)
			.ToImmutableList();
}
