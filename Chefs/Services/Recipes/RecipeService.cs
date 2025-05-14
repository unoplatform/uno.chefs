using Chefs.Services.Clients;
using ReviewData = Chefs.Services.Clients.Models.ReviewData;

namespace Chefs.Services.Recipes;

public class RecipeService(
	ChefsApiClient api,
	IUserService userService,
	IWritableOptions<SearchHistory> searchOptions,
	IMessenger messenger)
	: IRecipeService
{
	private int _lastTextLength;

	public async ValueTask<IImmutableList<Recipe>> GetAll(CancellationToken ct)
	{
		var recipesData = await api.Api.Recipe.GetAsync(cancellationToken: ct);
		return recipesData?.Select(r => new Recipe(r)).ToImmutableList() ?? ImmutableList<Recipe>.Empty;
	}

	public async ValueTask<int> GetCount(Guid userId, CancellationToken ct)
	{
		var countData = await api.Api.Recipe.Count.GetAsync(q => q.QueryParameters.UserId = userId, cancellationToken: ct);
		return (int)countData;
	}

	public async ValueTask<IImmutableList<Recipe>> GetByCategory(int categoryId, CancellationToken ct)
	{
		var recipesData = await api.Api.Recipe.GetAsync(cancellationToken: ct);
		return recipesData?.Where(r => r.Category?.Id == categoryId).Select(r => new Recipe(r)).ToImmutableList() ?? ImmutableList<Recipe>.Empty;
	}

	public async ValueTask<IImmutableList<Category>> GetCategories(CancellationToken ct)
	{
		var categoriesData = await api.Api.Recipe.Categories.GetAsync(cancellationToken: ct);
		return categoriesData?.Select(c => new Category(c)).ToImmutableList() ?? ImmutableList<Category>.Empty;
	}

	public async ValueTask<IImmutableList<CategoryWithCount>> GetCategoriesWithCount(CancellationToken ct)
	{
		var categories = await GetCategories(ct);
		var tasks = categories.Select(async category =>
		{
			var recipesByCategory = await GetByCategory(category.Id ?? 0, ct);
			return new CategoryWithCount(recipesByCategory.Count, category);
		});

		var categoriesWithCount = await Task.WhenAll(tasks);
		return categoriesWithCount.ToImmutableList();
	}

	public async ValueTask<IImmutableList<Recipe>> GetRecent(CancellationToken ct)
	{
		var recipesData = await api.Api.Recipe.GetAsync(cancellationToken: ct);
		return recipesData?.Select(r => new Recipe(r)).OrderByDescending(x => x.Date).Take(7).ToImmutableList() ?? ImmutableList<Recipe>.Empty;
	}

	public async ValueTask<IImmutableList<Recipe>> GetTrending(CancellationToken ct)
	{
		var trendingRecipesData = await api.Api.Recipe.Trending.GetAsync(cancellationToken: ct);
		return trendingRecipesData?.Select(r => new Recipe(r)).ToImmutableList() ?? ImmutableList<Recipe>.Empty;
	}

	public async ValueTask<IImmutableList<Recipe>> GetPopular(CancellationToken ct)
	{
		var popularRecipesData = await api.Api.Recipe.Popular.GetAsync(cancellationToken: ct);
		return popularRecipesData?.Select(r => new Recipe(r)).ToImmutableList() ?? ImmutableList<Recipe>.Empty;
	}

	public async ValueTask<IImmutableList<Recipe>> Search(string term, SearchFilter filter, CancellationToken ct)
	{
		var recipesToSearch = filter.FilterGroup switch
		{
			FilterGroup.Popular => await GetPopular(ct),
			FilterGroup.Trending => await GetTrending(ct),
			FilterGroup.Recent => await GetRecent(ct),
			_ => await GetAll(ct)
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
		var reviewsData = await api.Api.Recipe[recipeId].Reviews.GetAsync(cancellationToken: ct);
		return reviewsData?.Select(x => new Review(x)).ToImmutableList() ?? ImmutableList<Review>.Empty;
	}

	public async ValueTask<IImmutableList<Step>> GetSteps(Guid recipeId, CancellationToken ct)
	{
		var stepsData = await api.Api.Recipe[recipeId].Steps.GetAsync(cancellationToken: ct);
		return stepsData?.Select(x => new Step(x)).ToImmutableList() ?? ImmutableList<Step>.Empty;
	}

	public async ValueTask<IImmutableList<Ingredient>> GetIngredients(Guid recipeId, CancellationToken ct)
	{
		var ingredientsData = await api.Api.Recipe[recipeId].Ingredients.GetAsync(cancellationToken: ct);
		return ingredientsData?.Select(x => new Ingredient(x)).ToImmutableList() ?? ImmutableList<Ingredient>.Empty;
	}

	public async ValueTask<IImmutableList<Recipe>> GetByUser(Guid userId, CancellationToken ct)
	{
		var recipesData = await api.Api.Recipe.GetAsync(cancellationToken: ct);
		return recipesData?.Where(r => r.UserId == userId).Select(x => new Recipe(x)).ToImmutableList() ?? ImmutableList<Recipe>.Empty;
	}

	public async ValueTask<Review> CreateReview(Guid recipeId, string review, CancellationToken ct)
	{
		var reviewData = new ReviewData { RecipeId = recipeId, Description = review };
		var createdReviewData = await api.Api.Recipe.Review.PostAsync(reviewData, cancellationToken: ct);
		return new Review(createdReviewData);
	}

	public IListState<Recipe> FavoritedRecipes => ListState<Recipe>.Async(this, GetFavorited);

	public async ValueTask<IImmutableList<Recipe>> GetFavoritedWithPagination(uint pageSize, uint firstItemIndex, CancellationToken ct)
	{
		var favoritedRecipes = await GetFavorited(ct);
		return favoritedRecipes
			.Skip((int)firstItemIndex)
			.Take((int)pageSize)
			.ToImmutableList();
	}

	public async ValueTask Favorite(Recipe recipe, CancellationToken ct)
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
		var reviewData = review.ToData();
		var updatedReviewData = await api.Api.Recipe.Review.Like.PostAsync(reviewData, cancellationToken: ct);
		var updatedReview = new Review(updatedReviewData);
		messenger.Send(new EntityMessage<Review>(EntityChange.Updated, updatedReview));
	}

	public async ValueTask DislikeReview(Review review, CancellationToken ct)
	{
		var reviewData = review.ToData();
		var updatedReviewData = await api.Api.Recipe.Review.Dislike.PostAsync(reviewData, cancellationToken: ct);
		var updatedReview = new Review(updatedReviewData);
		messenger.Send(new EntityMessage<Review>(EntityChange.Updated, updatedReview));
	}

	public async ValueTask<IImmutableList<Recipe>> GetRecommended(CancellationToken ct)
	{
		var recipesData = await api.Api.Recipe.GetAsync(cancellationToken: ct);
		return recipesData?.Select(r => new Recipe(r)).OrderBy(_ => Guid.NewGuid()).Take(4).ToImmutableList() ?? ImmutableList<Recipe>.Empty;
	}

	public async ValueTask<IImmutableList<Recipe>> GetFromChefs(CancellationToken ct)
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
