using System.Text.Json;
using Chefs.Services.Clients;
using Microsoft.Kiota.Abstractions.Serialization;
using CategoryData = Chefs.Services.Clients.Models.CategoryData;
using IngredientData = Chefs.Services.Clients.Models.IngredientData;
using RecipeData = Chefs.Services.Clients.Models.RecipeData;
using ReviewData = Chefs.Services.Clients.Models.ReviewData;
using StepData = Chefs.Services.Clients.Models.StepData;

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
		await using var responseStream = await api.Api.Recipe.GetAsync(cancellationToken: ct);
		var jsonResponse = await new StreamReader(responseStream).ReadToEndAsync(ct);
		var recipesData = await KiotaJsonSerializer.DeserializeCollectionAsync<RecipeData>(jsonResponse, cancellationToken: ct);
		return recipesData?.Select(r => new Recipe(r)).ToImmutableList() ?? ImmutableList<Recipe>.Empty;
	}

	public async ValueTask<int> GetCount(Guid userId, CancellationToken ct)
	{
		await using var responseStream = await api.Api.Recipe.Count.GetAsync(q => q.QueryParameters.UserId = userId, cancellationToken: ct);
		var jsonResponse = await new StreamReader(responseStream).ReadToEndAsync(ct);
		var countData = JsonSerializer.Deserialize<int>(jsonResponse);
		return countData;
	}

	public async ValueTask<IImmutableList<Recipe>> GetByCategory(int categoryId, CancellationToken ct)
	{
		await using var responseStream = await api.Api.Recipe.GetAsync(cancellationToken: ct);
		var jsonResponse = await new StreamReader(responseStream).ReadToEndAsync(ct);
		var recipesData = await KiotaJsonSerializer.DeserializeCollectionAsync<RecipeData>(jsonResponse, cancellationToken: ct);
		return recipesData?.Where(r => r.Category?.Id == categoryId).Select(r => new Recipe(r)).ToImmutableList() ?? ImmutableList<Recipe>.Empty;
	}

	public async ValueTask<IImmutableList<Category>> GetCategories(CancellationToken ct)
	{
		await using var responseStream = await api.Api.Recipe.Categories.GetAsync(cancellationToken: ct);
		var jsonResponse = await new StreamReader(responseStream).ReadToEndAsync(ct);
		var categoriesData = await KiotaJsonSerializer.DeserializeCollectionAsync<CategoryData>(jsonResponse, cancellationToken: ct);
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
		await using var responseStream = await api.Api.Recipe.GetAsync(cancellationToken: ct);
		var jsonResponse = await new StreamReader(responseStream).ReadToEndAsync(ct);
		var recipesData = await KiotaJsonSerializer.DeserializeCollectionAsync<RecipeData>(jsonResponse, cancellationToken: ct);
		return recipesData?.Select(r => new Recipe(r)).OrderByDescending(x => x.Date).Take(7).ToImmutableList() ?? ImmutableList<Recipe>.Empty;
	}

	public async ValueTask<IImmutableList<Recipe>> GetTrending(CancellationToken ct)
	{
		await using var responseStream = await api.Api.Recipe.Trending.GetAsync(cancellationToken: ct);
		var jsonResponse = await new StreamReader(responseStream).ReadToEndAsync(ct);
		var trendingRecipesData = await KiotaJsonSerializer.DeserializeCollectionAsync<RecipeData>(jsonResponse, cancellationToken: ct);
		return trendingRecipesData?.Select(r => new Recipe(r)).ToImmutableList() ?? ImmutableList<Recipe>.Empty;
	}

	public async ValueTask<IImmutableList<Recipe>> GetPopular(CancellationToken ct)
	{
		await using var responseStream = await api.Api.Recipe.Popular.GetAsync(cancellationToken: ct);
		var jsonResponse = await new StreamReader(responseStream).ReadToEndAsync(ct);
		var popularRecipesData = await KiotaJsonSerializer.DeserializeCollectionAsync<RecipeData>(jsonResponse, cancellationToken: ct);
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
		await using var responseStream = await api.Api.Recipe[recipeId].Reviews.GetAsync(cancellationToken: ct);
		var jsonResponse = await new StreamReader(responseStream).ReadToEndAsync(ct);
		var reviewsData = await KiotaJsonSerializer.DeserializeCollectionAsync<ReviewData>(jsonResponse, cancellationToken: ct);
		return reviewsData?.Select(x => new Review(x)).ToImmutableList() ?? ImmutableList<Review>.Empty;
	}
	
	public async ValueTask<IImmutableList<Step>> GetSteps(Guid recipeId, CancellationToken ct)
	{
		await using var responseStream = await api.Api.Recipe[recipeId].Steps.GetAsync(cancellationToken: ct);
		var jsonResponse = await new StreamReader(responseStream).ReadToEndAsync(ct);
		var stepsData = await KiotaJsonSerializer.DeserializeCollectionAsync<StepData>(jsonResponse, cancellationToken: ct);
		return stepsData?.Select(x => new Step(x)).ToImmutableList() ?? ImmutableList<Step>.Empty;
	}
	
	public async ValueTask<IImmutableList<Ingredient>> GetIngredients(Guid recipeId, CancellationToken ct)
	{
		await using var responseStream = await api.Api.Recipe[recipeId].Ingredients.GetAsync(cancellationToken: ct);
		var jsonResponse = await new StreamReader(responseStream).ReadToEndAsync(ct);
		var ingredientsData = await KiotaJsonSerializer.DeserializeCollectionAsync<IngredientData>(jsonResponse, cancellationToken: ct);
		return ingredientsData?.Select(x => new Ingredient(x)).ToImmutableList() ?? ImmutableList<Ingredient>.Empty;
	}

	public async ValueTask<IImmutableList<Recipe>> GetByUser(Guid userId, CancellationToken ct)
	{
		await using var responseStream = await api.Api.Recipe.GetAsync(cancellationToken: ct);
		var jsonResponse = await new StreamReader(responseStream).ReadToEndAsync(ct);
		var recipesData = await KiotaJsonSerializer.DeserializeCollectionAsync<RecipeData>(jsonResponse, cancellationToken: ct);
		return recipesData?.Where(r => r.UserId == userId).Select(x => new Recipe(x)).ToImmutableList() ?? ImmutableList<Recipe>.Empty;
	}

	public async ValueTask<Review> CreateReview(Guid recipeId, string review, CancellationToken ct)
	{
		var reviewData = new ReviewData { RecipeId = recipeId, Description = review };
		await using var responseStream = await api.Api.Recipe.Review.PostAsync(reviewData);
		var jsonResponse = await new StreamReader(responseStream).ReadToEndAsync(ct);
		var createdReviewData = await KiotaJsonSerializer.DeserializeAsync<ReviewData>(jsonResponse, cancellationToken: ct);
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
			await FavoritedRecipes.AddAsync(updatedRecipe);
		}
		else
		{
			await FavoritedRecipes.RemoveAllAsync(r => r.Id == updatedRecipe.Id);
		}

		messenger.Send(new EntityMessage<Recipe>(EntityChange.Updated, updatedRecipe));
	}

	public async ValueTask LikeReview(Review review, CancellationToken ct)
	{
		var reviewData = review.ToData();
		await using var responseStream = await api.Api.Recipe.Review.Like.PostAsync(reviewData);
		var jsonResponse = await new StreamReader(responseStream).ReadToEndAsync(ct);
		var updatedReviewData = await KiotaJsonSerializer.DeserializeAsync<ReviewData>(jsonResponse, cancellationToken: ct);
		var updatedReview = new Review(updatedReviewData);
		messenger.Send(new EntityMessage<Review>(EntityChange.Updated, updatedReview));
	}

	public async ValueTask DislikeReview(Review review, CancellationToken ct)
	{
		var reviewData = review.ToData();
		await using var responseStream = await api.Api.Recipe.Review.Dislike.PostAsync(reviewData);
		var jsonResponse = await new StreamReader(responseStream).ReadToEndAsync(ct);
		var updatedReviewData = await KiotaJsonSerializer.DeserializeAsync<ReviewData>(jsonResponse, cancellationToken: ct);
		var updatedReview = new Review(updatedReviewData);
		messenger.Send(new EntityMessage<Review>(EntityChange.Updated, updatedReview));
	}

	public async ValueTask<IImmutableList<Recipe>> GetRecommended(CancellationToken ct)
	{
		await using var responseStream = await api.Api.Recipe.GetAsync(cancellationToken: ct);
		var jsonResponse = await new StreamReader(responseStream).ReadToEndAsync(ct);
		var recipesData = await KiotaJsonSerializer.DeserializeCollectionAsync<RecipeData>(jsonResponse, cancellationToken: ct);
		return recipesData?.Select(r => new Recipe(r)).OrderBy(_ => Guid.NewGuid()).Take(4).ToImmutableList() ?? ImmutableList<Recipe>.Empty;
	}

	public async ValueTask<IImmutableList<Recipe>> GetFromChefs(CancellationToken ct)
	{
		await using var responseStream = await api.Api.Recipe.GetAsync(cancellationToken: ct);
		var jsonResponse = await new StreamReader(responseStream).ReadToEndAsync(ct);
		var recipesData = await KiotaJsonSerializer.DeserializeCollectionAsync<RecipeData>(jsonResponse, cancellationToken: ct);
		return recipesData?.Select(r => new Recipe(r)).OrderBy(_ => Guid.NewGuid()).Take(4).ToImmutableList() ?? ImmutableList<Recipe>.Empty;
	}

	private async ValueTask<IImmutableList<Recipe>> GetFavorited(CancellationToken ct)
	{
		var currentUser = await userService.GetCurrent(ct);
		await using var responseStream = await api.Api.Recipe.Favorited.GetAsync(config => config.QueryParameters.UserId = currentUser.Id, cancellationToken: ct);
		var jsonResponse = await new StreamReader(responseStream).ReadToEndAsync(ct);
		var favoritedRecipesData = await KiotaJsonSerializer.DeserializeCollectionAsync<RecipeData>(jsonResponse, cancellationToken: ct);
		return favoritedRecipesData?.Select(r => new Recipe(r)).ToImmutableList() ?? ImmutableList<Recipe>.Empty;
	}

	private async Task SaveSearchHistory(string text)
	{
		if (_lastTextLength <= text.Length) _lastTextLength = text.Length;

		var searchHistory = searchOptions.Value.Searches;
		if (searchHistory is not null && !string.IsNullOrWhiteSpace(text))
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
