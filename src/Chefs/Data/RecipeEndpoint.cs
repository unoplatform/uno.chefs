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

    private List<SavedRecipesData>? _savedRecipes;
    private List<RecipeData>? _recipes;
    private List<CategoryData>? _categories;

    public RecipeEndpoint(IStorage dataService, ISerializer serializer, IUserEndpoint userEndpoint)
        => (_dataService, _serializer, _userEndpoint) = (dataService, serializer, userEndpoint);

    public async ValueTask<IImmutableList<RecipeData>> GetAll(CancellationToken ct) => (await Load()).ToImmutableList()
        ?? ImmutableList<RecipeData>.Empty;

    public async ValueTask<IImmutableList<CategoryData>> GetCategories(CancellationToken ct) => (await LoadCategories())
        .ToImmutableList()
        ?? ImmutableList<CategoryData>.Empty;

    public async ValueTask<IImmutableList<RecipeData>> GetTrending(CancellationToken ct) => (await Load())?
        .Take(10)
        .ToImmutableList()
        ?? ImmutableList<RecipeData>.Empty;

    public async ValueTask<IImmutableList<RecipeData>> GetSaved(CancellationToken ct)
    {
        var currentUser = await _userEndpoint.GetCurrent(ct);

        var recipes = await Load();

        var savedRecipes = (await LoadSaved())?
            .Where(x => x.UserId == currentUser.Id).FirstOrDefault();

        if (savedRecipes is not null && savedRecipes.SavedRecipes is not null)
        {
            return recipes?.Where(x => savedRecipes.SavedRecipes.Any(y => y == x.Id)).ToImmutableList() ?? ImmutableList<RecipeData>.Empty;
        }

        return ImmutableList<RecipeData>.Empty;
    }

    public async ValueTask Save(RecipeData recipe, CancellationToken ct)
    {
        var currentUser = await _userEndpoint.GetCurrent(ct);
        
        var savedRecipes = await LoadSaved();

        var userSavedRecipe = savedRecipes?.Where(x => x.UserId == currentUser.Id).FirstOrDefault();

        if (userSavedRecipe is not null && userSavedRecipe.SavedRecipes is not null)
        {
            userSavedRecipe.SavedRecipes = !userSavedRecipe.SavedRecipes.Contains(recipe.Id) ? 
                userSavedRecipe.SavedRecipes.Concat(recipe.Id).ToArray() :
                userSavedRecipe.SavedRecipes;
        }
        else
        {
            savedRecipes?.Add(new SavedRecipesData { UserId = currentUser.Id, SavedRecipes = new Guid[] { recipe.Id } });
        }

        _savedRecipes= savedRecipes!;
    }

    public async ValueTask<ReviewData> CreateReview(ReviewData reviewData, CancellationToken ct)
    {
        var currentUser = await _userEndpoint.GetCurrent(ct);

        var recipes = await Load();

        var recipe = recipes.Where(r => r.Id == reviewData.RecipeId).FirstOrDefault();

        if(recipe is not null)
        {
            reviewData.PublisherName = currentUser.FullName;
            reviewData.UrlAuthorImage = currentUser.UrlProfileImage;
            reviewData.CreatedBy = currentUser.Id;
            reviewData.Date = DateTime.Now;
            recipe.Reviews?.Add(reviewData);

            return reviewData;
        }
        else
        {
            throw new Exception();
        }
    }

    //Implementation to update saved recipes in memory 
    private async ValueTask<IList<RecipeData>> Load()
    {
        if(_recipes == null)
        {
            _recipes = (await _dataService
                .ReadPackageFileAsync<List<RecipeData>>(_serializer, Constants.RecipeDataFile));
            var saved = await GetSaved(CancellationToken.None);

            _recipes?.ForEach(x => x.Save = saved.Contains(x));
        }

        return _recipes ?? new List<RecipeData>();
    }

    //Implementation to update saved cookbooks and recipes in memory 
    private async ValueTask<List<SavedRecipesData>> LoadSaved()
    {
        if (_savedRecipes == null)
        {
            _savedRecipes = (await _dataService
                .ReadPackageFileAsync<List<SavedRecipesData>>(_serializer, Constants.SavedRecipesDataFile));
        }
        return _savedRecipes ?? new List<SavedRecipesData>();
    }

    //Implementation categories to keep in memory 
    private async ValueTask<List<CategoryData>> LoadCategories()
    {
        if (_categories == null)
        {
            _categories = (await _dataService
                .ReadPackageFileAsync<List<CategoryData>>(_serializer, Constants.CategoryDataFile));
        }
        return _categories ?? new List<CategoryData>();
    }
}
