using Chefs.Data;
using System.Collections.Immutable;

namespace Chefs.Business;

public class RecipeService : IRecipeService
{
    private readonly IRecipeEndpoint _recipeEndpoint;
    private readonly ICookbookEndpoint _cookbookEndpoint;
    private readonly IUserEndpoint _userEndpoint;

    public RecipeService(IRecipeEndpoint recipeEndpoint, IUserEndpoint userEndpoint, ICookbookEndpoint cookbookEndpoint) 
        => (_recipeEndpoint, _userEndpoint, _cookbookEndpoint) = (recipeEndpoint, userEndpoint, cookbookEndpoint);

    public async ValueTask<IImmutableList<Recipe>> GetAll(CancellationToken ct) => (await _recipeEndpoint
                   .GetAll((await _userEndpoint.GetUser(ct)).Id, ct))
                   .Select(r => new Recipe(r))
                   .ToImmutableList();
    

    public async ValueTask<IImmutableList<Recipe>> GetByCategory(int categoryId, CancellationToken ct) => (await _recipeEndpoint
                   .GetAll((await _userEndpoint.GetUser(ct)).Id, ct))
                   .Select(r => new Recipe(r))
                   .Where(r => r.Category?.Id == categoryId)
                   .ToImmutableList();

    public async ValueTask<IImmutableList<Category>> GetCategories(CancellationToken ct) => (await _recipeEndpoint.GetCategories(ct))
                   .Select(c => new Category(c))
                   .ToImmutableList();
    

    public async ValueTask<IImmutableList<Cookbook>> GetSavedCookbooks(CancellationToken ct)=> (await _cookbookEndpoint.GetSavedCookbooks(ct))
                   .Select(c => new Cookbook(c))
                   .ToImmutableList();

    public async ValueTask<IImmutableList<User>> GetPopularCreators(CancellationToken ct) => (await _userEndpoint.GetPopularCreators(ct))
                   .Select(u => new User(u))
                   .ToImmutableList();

    public async ValueTask<IImmutableList<Recipe>> GetRecent(CancellationToken ct) => (await _recipeEndpoint
                   .GetAll((await _userEndpoint.GetUser(ct)).Id, ct))
                   .Select(r => new Recipe(r))
                   .Where(r => r.Date == DateTime.Now)
                   .ToImmutableList();

    public async ValueTask<IImmutableList<Recipe>> GetTrending(CancellationToken ct) => (await _recipeEndpoint
                   .GetTrending((await _userEndpoint.GetUser(ct)).Id, ct))
                   .Select(r => new Recipe(r))
                   .ToImmutableList();

    public async ValueTask<IImmutableList<Recipe>> Search(string term, CancellationToken ct)
    {
        var recipes = (await _recipeEndpoint
                   .GetAll((await _userEndpoint.GetUser(ct)).Id, ct))
                   .Select(r => new Recipe(r))
                   .ToImmutableList();
        return GetRecipesByText(recipes.ToList(), term);
    }

    private IImmutableList<Recipe> GetRecipesByText(List<Recipe> recipes, string? text) => recipes
            .Where(r => text == null 
            || r.Name?.ToLower() == text.ToLower() 
            || r.Category?.Name?.ToLower() == text.ToLower()).ToImmutableList();

}
