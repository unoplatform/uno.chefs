using Chefs.Data;
using System.Collections.Immutable;

namespace Chefs.Business;

public class RecipeService : IRecipeService
{
    private readonly IRecipeEndpoint _recipeEndpoint;

    public RecipeService(IRecipeEndpoint recipeEndpoint)
    {
        _recipeEndpoint = recipeEndpoint;
    }

    public async ValueTask<IImmutableList<Recipe>> GetAll(CancellationToken ct)
    {
        return (await _recipeEndpoint.GetAll(ct))
                   .Select(r => new Recipe(r))
                   .ToImmutableList();
    }

    public async ValueTask<IImmutableList<Recipe>> GetByCategory(Category category, CancellationToken ct)
    {
        return (await _recipeEndpoint.GetAll(ct))
                   .Select(r => new Recipe(r))
                   .Where(r => r.Category?.Id == category.Id)
                   .ToImmutableList();
    }

    public async ValueTask<IImmutableList<Category>> GetCategories(CancellationToken ct)
    {
        return (await _recipeEndpoint.GetCategories(ct))
                   .Select(c => new Category(c))
                   .ToImmutableList();
    }

    public async ValueTask<IImmutableList<Cookbook>> GetCookbooks(CancellationToken ct)
    {
        return (await _recipeEndpoint.GetCookbooks(ct))
                   .Select(c => new Cookbook(c))
                   .ToImmutableList();
    }

    public async ValueTask<IImmutableList<User>> GetPopularCreators(CancellationToken ct)
    {
        return (await _recipeEndpoint.GetPopularCreators(ct))
                   .Select(u => new User(u))
                   .ToImmutableList();
    }

    public async ValueTask<IImmutableList<Recipe>> GetRecent(CancellationToken ct)
    {
        return (await _recipeEndpoint.GetAll(ct))
                   .Select(r => new Recipe(r))
                   .Where(r => r.Date == DateTime.Now)
                   .ToImmutableList();
    }

    public async ValueTask<IImmutableList<Recipe>> GetTrending(CancellationToken ct)
    {
        return (await _recipeEndpoint.GetAll(ct))
                   .Select(r => new Recipe(r))
                   .Where(r => r.Date == DateTime.Now && r.Reviews?.Count > 0)
                   .ToImmutableList();
    }

    public ValueTask<IImmutableList<Recipe>> Search(SearchFilter search, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}
