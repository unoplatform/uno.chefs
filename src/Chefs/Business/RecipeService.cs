using Chefs.Data;
using System.Collections.Immutable;

namespace Chefs.Business;

public class RecipeService : IRecipeService
{
    private readonly IRecipeEndpoint _recipeEndpoint;
    private readonly IUserEndpoint _userEndpoint;

    public RecipeService(IRecipeEndpoint recipeEndpoint, IUserEndpoint userEndpoint) 
        => (_recipeEndpoint, _userEndpoint) = (recipeEndpoint, userEndpoint);

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
                   .Where(r => r.Date > DateTime.Now.AddDays(7))
                   .ToImmutableList();

    public async ValueTask<IImmutableList<Recipe>> GetTrending(CancellationToken ct) => (await _recipeEndpoint
                   .GetTrending(ct))
                   .Select(r => new Recipe(r))
                   .ToImmutableList();

    public async ValueTask<IImmutableList<Recipe>> Search(string term, CancellationToken ct) => GetRecipesByText((await _recipeEndpoint
                   .GetAll(ct))
                   .Select(r => new Recipe(r)), term);


    private IImmutableList<Recipe> GetRecipesByText(IEnumerable<Recipe> recipes, string? text) => recipes
            .Where(r => text == null 
            || r.Name?.ToLower() == text.ToLower() 
            || r.Category?.Name?.ToLower() == text.ToLower()).ToImmutableList();

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

    public async ValueTask<IImmutableList<Recipe>> GetByUser(Guid userId, CancellationToken ct) =>
        (await _recipeEndpoint.GetAll(ct))
        .Where(r=>r.UserId == userId)
        .Select(x => new Recipe(x))
        .ToImmutableList() ?? ImmutableList<Recipe>.Empty;
}
