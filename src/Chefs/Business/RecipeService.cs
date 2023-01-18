using Chefs.Data;
using System.Collections.Immutable;
using System.Diagnostics;
using Uno.Extensions.Configuration;

namespace Chefs.Business;

public class RecipeService : IRecipeService
{
    private readonly IRecipeEndpoint _recipeEndpoint;
    private readonly IWritableOptions<SearchHistory> _searchOptions;
    private Signal _refreshRecipes = new();

    public RecipeService(IRecipeEndpoint recipeEndpoint, IWritableOptions<SearchHistory> searchHistory) 
        => (_recipeEndpoint, _searchOptions) = (recipeEndpoint, searchHistory);

    // DR_REV: Global to the file: Aligment
    //  * Only one tab per new line
    //  * When possible keep a code scope on a single line
    //     i.e. (1 line) "(await _recipeEndpoint.GetAll(ct)) "
    //     instead of (2-lines) "(await _recipeEndpoint
    //          .GetAll(ct))"
    //  * A change of scope should involve a new line with one more or less indentation level
    //     i.e.
    //          "(await _recipeEndpoint.GetAll(ct))
    //              .Select()..."
    //      instead of 
    //          "(await _recipeEndpoint.GetAll(ct))
    //          .Select()"
    //  * When using expression body, put the body on a new line.
    //  * When splitting a scope with new lines, add a new line for each statement
    //      i.e. 
    //		    .FirstOrDefault(r => r.Id == recipeId)
    //		    ?.Steps
    //		    ?.Select(x => new Step(x))
    //      instead of
    //          .FirstOrDefault(r => r.Id == recipeId)?.Ingredients?
    //          .Select(x => new Ingredient(x))

    public async ValueTask<IImmutableList<Recipe>> GetAll(CancellationToken ct) 
        => (await _recipeEndpoint.GetAll(ct))
           .Select(r => new Recipe(r))
           .ToImmutableList();

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
           .OrderBy(x=>x.Date)
           .Take(5)
           .ToImmutableList();

    public async ValueTask<IImmutableList<Recipe>> GetTrending(CancellationToken ct) 
        => (await _recipeEndpoint.GetTrending(ct))
           .Select(r => new Recipe(r))
           .ToImmutableList();

    public async ValueTask<IImmutableList<Recipe>> Search(string term, CancellationToken ct)
    {
        if (term.IsNullOrEmpty())
        {
            return (await _recipeEndpoint.GetAll(ct)).Select(r => new Recipe(r)).ToImmutableList();
        }
        else
        {
            await SaveSearchHistory(term);
            return GetRecipesByText((await _recipeEndpoint.GetAll(ct))
                       .Select(r => new Recipe(r)), term);
        }
    }

    public IImmutableList<string> GetSearchHistory() => (_searchOptions.Value).Searches.Reverse().Take(3).ToImmutableList();

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
            .Where(r=>r.UserId == userId)
            .Select(x => new Recipe(x))
            .ToImmutableList(); // DR_REV: Cannot be null

    public async ValueTask<Review> CreateReview(Guid recipeId, string review, CancellationToken ct) 
        => new(await _recipeEndpoint.CreateReview(new ReviewData { RecipeId = recipeId, Description = review }, ct));


    public IListFeed<Recipe> SavedRecipes => ListFeed<Recipe>.Async(async ct => await GetSaved(ct), _refreshRecipes);

    public async ValueTask<IImmutableList<Recipe>> GetSaved(CancellationToken ct)
        => (await _recipeEndpoint.GetSaved(ct))
            .Select(r => new Recipe(r))
            .ToImmutableList();

    public async ValueTask Save(Recipe recipe, CancellationToken ct)
    {
        await _recipeEndpoint.Save(recipe.ToData(), ct);
        _refreshRecipes.Raise();
    }

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
        var searchHistory = _searchOptions.Value.Searches;
        if (searchHistory is not null && !text.IsNullOrEmpty())
        {
            if(searchHistory.Count == 0)
            {
                await _searchOptions.UpdateAsync(h => h with { Searches = searchHistory.Add(text) });
            }
            else if ((text.Contains(searchHistory.LastOrDefault()!) || searchHistory.LastOrDefault()!.Contains(text)))
            {
                await _searchOptions.UpdateAsync(h => h with { Searches = searchHistory.Replace(searchHistory.LastOrDefault() 
                    ?? string.Empty, text) });
            }
        }
    }

    private IImmutableList<Recipe> GetRecipesByText(IEnumerable<Recipe> recipes, string text)
        => recipes
            .Where(r => r.Name!.ToLower().Contains(text.ToLower())
                || r.Category!.Name!.ToLower().Contains(text.ToLower())).ToImmutableList();
}
