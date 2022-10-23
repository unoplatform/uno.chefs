using Chefs.Data.Models;
using System.Collections.Immutable;
using Uno.Extensions.Serialization;
using Uno.Extensions.Storage;

namespace Chefs.Data;

public class RecipeEndpoint : IRecipeEndpoint
{
    private readonly IStorage _dataService;
    private readonly ISerializer _serializer;
    private readonly IUserEndpoint _userEndpoint;

    public RecipeEndpoint(IStorage dataService, ISerializer serializer, IUserEndpoint userEndpoint)
        => (_dataService, _serializer, _userEndpoint) = (dataService, serializer, userEndpoint);

    public async ValueTask<IImmutableList<RecipeData>> GetAll(CancellationToken ct) => await _dataService
        .ReadFileAsync<IImmutableList<RecipeData>>(_serializer, Constants.RecipeDataFile) 
        ?? ImmutableList<RecipeData>.Empty;

    public async ValueTask<IImmutableList<CategoryData>> GetCategories(CancellationToken ct) => await _dataService
        .ReadFileAsync<IImmutableList<CategoryData>>(_serializer, Constants.CategoryDataFile)
        ?? ImmutableList<CategoryData>.Empty;

    public async ValueTask<IImmutableList<RecipeData>> GetTrending(CancellationToken ct) => (await _dataService
        .ReadFileAsync<IImmutableList<RecipeData>>(_serializer, Constants.RecipeDataFile))?
        .Take(10)
        .ToImmutableList()
        ?? ImmutableList<RecipeData>.Empty;

    public async ValueTask<IImmutableList<RecipeData>> GetSaved(CancellationToken ct)
    {
        var currentUser = await _userEndpoint.GetUser(ct);

        var recipes = (await _dataService
            .ReadFileAsync<List<RecipeData>>(_serializer, Constants.RecipeDataFile));

        var savedRecipes = (await _dataService
            .ReadFileAsync<List<SavedItemsData>>(_serializer, Constants.SavedItemsDataFile))?
            .Where(x => x.UserId == currentUser.Id).FirstOrDefault();

        if (savedRecipes is not null && savedRecipes.SavedRecipes is not null)
        {
            return recipes?.Where(x => savedRecipes.SavedRecipes.Any(y => y == x.Id)).ToImmutableList() ?? ImmutableList<RecipeData>.Empty;
        }

        return ImmutableList<RecipeData>.Empty;
    }
}
