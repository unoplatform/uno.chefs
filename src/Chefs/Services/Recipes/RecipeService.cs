namespace Chefs.Services.Recipes;

public class RecipeService : IRecipeService
{
	private readonly IRecipeEndpoint _recipeEndpoint;
	private readonly IWritableOptions<SearchHistory> _searchOptions;
	private readonly IMessenger _messenger;
	private int _lastTextLength = 0;

	public RecipeService(IRecipeEndpoint recipeEndpoint, IWritableOptions<SearchHistory> searchHistory, IMessenger messenger)
		=> (_recipeEndpoint, _searchOptions, _messenger) = (recipeEndpoint, searchHistory, messenger);

	public async ValueTask<IImmutableList<Recipe>> GetAll(CancellationToken ct)
		=> (await _recipeEndpoint.GetAll(ct))
		   .Select(r => new Recipe(r))
		   .ToImmutableList();

	public async ValueTask<int> GetCount(Guid userId, CancellationToken ct) => await _recipeEndpoint.GetCount(userId, ct);

	public async ValueTask<IImmutableList<Recipe>> GetByCategory(int categoryId, CancellationToken ct)
		=> (await _recipeEndpoint.GetAll(ct))
		   .Select(r => new Recipe(r))
		   .Where(r => r.Category?.Id == categoryId)
		   .ToImmutableList();

	public async ValueTask<IImmutableList<Category>> GetCategories(CancellationToken ct)
		=> (await _recipeEndpoint.GetCategories(ct))
		   .Select(c => new Category(c))
		   .ToImmutableList();

	public async ValueTask<IImmutableList<Recipe>> GetRecent(CancellationToken ct)
		=> (await _recipeEndpoint.GetAll(ct))
		   .Select(r => new Recipe(r))
		   .OrderBy(x => x.Date)
		   .Take(7)
		   .ToImmutableList();

	public async ValueTask<IImmutableList<Recipe>> GetTrending(CancellationToken ct)
		=> (await _recipeEndpoint.GetTrending(ct))
		   .Select(r => new Recipe(r))
		   .ToImmutableList();

	public async ValueTask<IImmutableList<Recipe>> GetPopular(CancellationToken ct)
		=> (await _recipeEndpoint.GetPopular(ct))
		   .Select(r => new Recipe(r))
		   .ToImmutableList();

	public async ValueTask<IImmutableList<Recipe>> Search(string term, CancellationToken ct)
	{
		if (term.IsNullOrEmpty())
		{
			_lastTextLength = 0;
			return (await _recipeEndpoint.GetAll(ct)).Select(r => new Recipe(r)).ToImmutableList();
		}
		else
		{
			await SaveSearchHistory(term);
			return GetRecipesByText((await _recipeEndpoint.GetAll(ct))
					   .Select(r => new Recipe(r)), term);
		}
	}

	public IImmutableList<string> GetSearchHistory() => _searchOptions.Value.Searches.Take(3).ToImmutableList();

	public async ValueTask<IImmutableList<Review>> GetReviews(Guid recipeId, CancellationToken ct)
		=> (await _recipeEndpoint.GetAll(ct))
			.FirstOrDefault(r => r.Id == recipeId)?.Reviews?
			.Select(x => new Review(x))
			.ToImmutableList() ?? ImmutableList<Review>.Empty;


	public async ValueTask<IImmutableList<Step>> GetSteps(Guid recipeId, CancellationToken ct)
		=> (await _recipeEndpoint.GetAll(ct))
			.FirstOrDefault(r => r.Id == recipeId)
			?.Steps
			?.Select(x => new Step(x))
			.ToImmutableList()
			?? ImmutableList<Step>.Empty;

	public async ValueTask<IImmutableList<Ingredient>> GetIngredients(Guid recipeId, CancellationToken ct)
		=> (await _recipeEndpoint.GetAll(ct))
			.FirstOrDefault(r => r.Id == recipeId)
			?.Ingredients
			?.Select(x => new Ingredient(x)).ToImmutableList()
			?? ImmutableList<Ingredient>.Empty;

	public async ValueTask<IImmutableList<Recipe>> GetByUser(Guid userId, CancellationToken ct)
		=> (await _recipeEndpoint.GetAll(ct))
			.Where(r => r.UserId == userId)
			.Select(x => new Recipe(x))
			.ToImmutableList(); // DR_REV: Cannot be null

	public async ValueTask<Review> CreateReview(Guid recipeId, string review, CancellationToken ct)
		=> new(await _recipeEndpoint.CreateReview(new ReviewData { RecipeId = recipeId, Description = review }, ct));


	public IListFeed<Recipe> SavedRecipes => ListFeed<Recipe>.Async(GetSaved);

	public async ValueTask<IImmutableList<Recipe>> GetSaved(CancellationToken ct)
		=> (await _recipeEndpoint.GetSaved(ct))
			.Select(r => new Recipe(r))
			.ToImmutableList();

	public async ValueTask Save(Recipe recipe, CancellationToken ct)
	{
		await _recipeEndpoint.Save(recipe.ToData(), ct);
		_messenger.Send(new EntityMessage<Recipe>(recipe.Save ? EntityChange.Created : EntityChange.Deleted, recipe));
	}

	public async ValueTask<Review> LikeReview(Review review, CancellationToken ct)
		=> new(await _recipeEndpoint.LikeReview(review.ToData(), ct));

	public async ValueTask<Review> DislikeReview(Review review, CancellationToken ct)
		=> new(await _recipeEndpoint.DislikeReview(review.ToData(), ct));

	public async ValueTask<IImmutableList<Recipe>> GetRecommended(CancellationToken ct)
		=> (await _recipeEndpoint.GetAll(ct))
		   .Select(r => new Recipe(r)).OrderBy(x => new Random(1).Next())
		   .Take(4)
		   .ToImmutableList();

	public async ValueTask<IImmutableList<Recipe>> GetFromChefs(CancellationToken ct)
		=> (await _recipeEndpoint.GetAll(ct))
		   .Select(r => new Recipe(r)).OrderBy(x => new Random(2).Next())
		   .Take(4)
		   .ToImmutableList();

	private async Task SaveSearchHistory(string text)
	{
		if (_lastTextLength <= text.Count()) _lastTextLength = text.Count();

		var searchHistory = _searchOptions.Value.Searches; 
		if (searchHistory is not null && !text.IsNullOrEmpty())
		{
			if (searchHistory.Count == 0 || _lastTextLength == 1)
			{
				await _searchOptions.UpdateAsync(h => h with { Searches = searchHistory.Prepend(text).ToList() });
			}
			else if (searchHistory.FirstOrDefault() is { } latestTerm
				&& (text.Contains(latestTerm) || latestTerm.Contains(text))
				&& _lastTextLength == text.Count())
			{
				await _searchOptions.UpdateAsync(h => h with
				{
					Searches = searchHistory.Skip(1).Prepend(text).ToList(),
				});
			}
		}
	}

	private IImmutableList<Recipe> GetRecipesByText(IEnumerable<Recipe> recipes, string text)
		=> recipes
			.Where(r => r.Name!.ToLower().Contains(text.ToLower())
				|| r.Category!.Name!.ToLower().Contains(text.ToLower())).ToImmutableList();
}
