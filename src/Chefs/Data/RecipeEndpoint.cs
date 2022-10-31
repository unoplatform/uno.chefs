using Chefs.Business;
using Chefs.Data.Models;
using System.Collections.Immutable;
using Uno.Extensions;
using Uno.Extensions.Serialization;
using Uno.Extensions.Storage;

namespace Chefs.Data;

public class RecipeEndpoint : IRecipeEndpoint
{
    private readonly IStorage _dataService;
    private readonly ISerializer _serializer;
    private readonly IUserEndpoint _userEndpoint;

    private IImmutableList<SavedRecipesData>? _savedRecipes;
    private IImmutableList<RecipeData>? _recipes;

    public RecipeEndpoint(IStorage dataService, ISerializer serializer, IUserEndpoint userEndpoint)
        => (_dataService, _serializer, _userEndpoint) = (dataService, serializer, userEndpoint);

    public async ValueTask<IImmutableList<RecipeData>> GetAll(CancellationToken ct) => await LoadRecipes()
        ?? ImmutableList<RecipeData>.Empty;

    public async ValueTask<IImmutableList<CategoryData>> GetCategories(CancellationToken ct) => await _dataService
        .ReadFileAsync<IImmutableList<CategoryData>>(_serializer, Constants.CategoryDataFile)
        ?? ImmutableList<CategoryData>.Empty;

    public async ValueTask<IImmutableList<RecipeData>> GetTrending(CancellationToken ct) => (await LoadRecipes())?
        .Take(10)
        .ToImmutableList()
        ?? ImmutableList<RecipeData>.Empty;

    public async ValueTask<IImmutableList<RecipeData>> GetSaved(CancellationToken ct)
    {
        var currentUser = await _userEndpoint.GetUser(ct);

        var recipes = await LoadRecipes();

        var savedRecipes = (await LoadSavedRecipes())?
            .Where(x => x.UserId == currentUser.Id).FirstOrDefault();

        if (savedRecipes is not null && savedRecipes.SavedRecipes is not null)
        {
            return recipes?.Where(x => savedRecipes.SavedRecipes.Any(y => y == x.Id)).ToImmutableList() ?? ImmutableList<RecipeData>.Empty;
        }

        return ImmutableList<RecipeData>.Empty;
    }

    public async ValueTask Save(RecipeData recipe, CancellationToken ct)
    {
        var currentUser = await _userEndpoint.GetUser(ct);

        var savedRecipes = (await LoadSavedRecipes()).ToList();

        var userSavedRecipe = savedRecipes?.Where(x => x.UserId == currentUser.Id).FirstOrDefault();

        if (userSavedRecipe is not null)
        {
            userSavedRecipe.SavedRecipes = userSavedRecipe.SavedRecipes.Concat(recipe.Id).ToArray();
        }
        else
        {
            savedRecipes?.Add(new SavedRecipesData { UserId = currentUser.Id, SavedRecipes = new Guid[] { recipe.Id } });
        }

        _savedRecipes= savedRecipes!.ToImmutableList();
    }

    public async ValueTask CreateReview(ReviewData reviewData, CancellationToken ct)
    {
        var currentUser = await _userEndpoint.GetUser(ct);

        var recipes = (await LoadRecipes()).ToList();

        var userRecipes = recipes.Where(r => r.Id == reviewData.RecipeId).FirstOrDefault();

        if(userRecipes is not null)
        {
            userRecipes.Reviews?.Add(reviewData);
        }
        else
        {
            throw new Exception();
        }
    }

    //Implementation to update saved recipes in memory 
    private async ValueTask<IImmutableList<RecipeData>> LoadRecipes()
    {
        if(_recipes == null)
        {
            _recipes = (await _dataService
                .ReadFileAsync<IImmutableList<RecipeData>>(_serializer, Constants.RecipeDataFile));
        }
        return _recipes ?? ImmutableList<RecipeData>.Empty;
    }

    //Implementation to update saved cookbooks and recipes in memory 
    private async ValueTask<IImmutableList<SavedRecipesData>> LoadSavedRecipes()
    {
        if (_savedRecipes == null)
        {
            _savedRecipes = (await _dataService
                .ReadFileAsync<IImmutableList<SavedRecipesData>>(_serializer, Constants.SavedRecipesDataFile));
        }
        return _savedRecipes ?? ImmutableList<SavedRecipesData>.Empty;
    }
}
