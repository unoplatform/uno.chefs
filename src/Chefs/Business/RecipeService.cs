using Chefs.Data;
using System.Collections.Immutable;
using System.Diagnostics;
using Uno.Extensions.Configuration;

namespace Chefs.Business;

public class RecipeService : IRecipeService
{
    private readonly IRecipeEndpoint _recipeEndpoint;
    private readonly IWritableOptions<SearchHistory> _searchOptions;

    public RecipeService(IRecipeEndpoint recipeEndpoint, IWritableOptions<SearchHistory> searchHistory) 
        => (_recipeEndpoint, _searchOptions) = (recipeEndpoint, searchHistory);

    public async ValueTask<IImmutableList<Recipe>> GetAll(CancellationToken ct) => (await _recipeEndpoint
                   .GetAll(ct))
                   .Select(r => new Recipe(r))
                   .ToImmutableList();

    public async ValueTask<IImmutableList<Recipe>> GetByCategory(int categoryId, CancellationToken ct) => (await _recipeEndpoint
                   .GetAll(ct))
                   .Select(r => new Recipe(r))
                   .Where(r => r.Category?.Id == categoryId)
                   .ToImmutableList();

    public async ValueTask<IImmutableList<Category>> GetCategories(CancellationToken ct) => (await _recipeEndpoint.GetCategories(ct))
                   .Select(c => new Category(c))
                   .ToImmutableList();

    public async ValueTask<IImmutableList<Recipe>> GetRecent(CancellationToken ct) => (await _recipeEndpoint
                   .GetAll(ct))
                   .Select(r => new Recipe(r))
                   .OrderBy(x=>x.Date)
                   .Take(5)
                   .ToImmutableList();

    public async ValueTask<IImmutableList<Recipe>> GetTrending(CancellationToken ct) => (await _recipeEndpoint
                   .GetTrending(ct))
                   .Select(r => new Recipe(r))
                   .ToImmutableList();

    public async ValueTask<IImmutableList<Recipe>> Search(string term, CancellationToken ct)
    {
        await SaveSearchHistory(term);
        return GetRecipesByText((await _recipeEndpoint
                   .GetAll(ct))
                   .Select(r => new Recipe(r)), term);
    }

    public IImmutableList<string> GetSearchHistory() => (_searchOptions.Value).Searches.Take(3).ToImmutableList();

    private async Task SaveSearchHistory(string text)
    {
        var searchHistory = _searchOptions.Value;
        if (searchHistory is not null && !text.IsNullOrEmpty())
        {
            var toAdd = text != searchHistory.Searches.LastOrDefault() ? searchHistory.Searches.Add(text) : searchHistory.Searches;
            await _searchOptions.UpdateAsync(h => h with { Searches = toAdd });
        }
    }

    private IImmutableList<Recipe> GetRecipesByText(IEnumerable<Recipe> recipes, string text) => recipes
            .Where(r => r.Name!.ToLower().Contains(text.ToLower())
            || r.Category!.Name!.ToLower().Contains(text.ToLower())).ToImmutableList();

    public async ValueTask<IImmutableList<Review>> GetReviews(Guid recipeId, CancellationToken ct) => 
        (await _recipeEndpoint.GetAll(ct))
        .FirstOrDefault(r => r.Id == recipeId)?.Reviews?
        .Select(x => new Review(x))
        .ToImmutableList() ?? ImmutableList<Review>.Empty;
    

    public async ValueTask<IImmutableList<Step>> GetSteps(Guid recipeId, CancellationToken ct) =>
        (await _recipeEndpoint.GetAll(ct))
        .FirstOrDefault(r => r.Id == recipeId)?.Steps?
        .Select(x => new Step(x))
        .ToImmutableList() ?? ImmutableList<Step>.Empty;

    public async ValueTask<IImmutableList<Ingredient>> GetIngredients(Guid recipeId, CancellationToken ct) =>
        (await _recipeEndpoint.GetAll(ct))
        .FirstOrDefault(r => r.Id == recipeId)?.Ingredients?
        .Select(x => new Ingredient(x))
        .ToImmutableList() ?? ImmutableList<Ingredient>.Empty;

    public async ValueTask<IImmutableList<Recipe>> GetByUser(Guid userId, CancellationToken ct) =>
        (await _recipeEndpoint.GetAll(ct))
        .Where(r=>r.UserId == userId)
        .Select(x => new Recipe(x))
        .ToImmutableList() ?? ImmutableList<Recipe>.Empty;

    public async ValueTask Save(Recipe recipe, CancellationToken ct) => await _recipeEndpoint
        .Save(recipe.ToData(), ct);


    public async ValueTask<Review> CreateReview(Guid recipeId, string review, CancellationToken ct) => new Review(await _recipeEndpoint
        .CreateReview(new ReviewData { RecipeId = recipeId, Description = review }, ct));

    public async ValueTask<IImmutableList<Recipe>> GetSaved(CancellationToken ct) => (await _recipeEndpoint
        .GetSaved(ct))
        .Select(r => new Recipe(r))
        .ToImmutableList();

    public async ValueTask<IImmutableList<Recipe>> GetRecommended(CancellationToken ct) => (await _recipeEndpoint
       .GetAll(ct))
       .Select(r => new Recipe(r)).OrderBy(x => (new Random()).Next())
       .Take(4)
       .ToImmutableList();
}
