﻿using Chefs.Data.Models;
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
    private IImmutableList<CategoryData>? _categories;

    public RecipeEndpoint(IStorage dataService, ISerializer serializer, IUserEndpoint userEndpoint)
        => (_dataService, _serializer, _userEndpoint) = (dataService, serializer, userEndpoint);

    public async ValueTask<IImmutableList<RecipeData>> GetAll(CancellationToken ct) => (await Load()).ToImmutableList()
        ?? ImmutableList<RecipeData>.Empty;

    public async ValueTask<IImmutableList<CategoryData>> GetCategories(CancellationToken ct) => await LoadCategories();

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

        if (userSavedRecipe is not null)
        {
            userSavedRecipe.SavedRecipes = userSavedRecipe.SavedRecipes.Concat(recipe.Id).ToArray();
        }
        else
        {
            savedRecipes?.Add(new SavedRecipesData { UserId = currentUser.Id, SavedRecipes = new Guid[] { recipe.Id } });
        }

        _savedRecipes= savedRecipes!;
    }

    public async ValueTask CreateReview(ReviewData reviewData, CancellationToken ct)
    {
        var currentUser = await _userEndpoint.GetCurrent(ct);

        var recipes = await Load();

        var recipe = recipes.Where(r => r.Id == reviewData.RecipeId).FirstOrDefault();

        if(recipe is not null)
        {
            reviewData.CreatedBy = currentUser.Id;
            recipe.Reviews?.Add(reviewData);
        }
        else
        {
            throw new Exception();
        }
    }

    //Implementation to update saved recipes in memory 
    private async ValueTask<List<RecipeData>> Load()
    {
        if(_recipes == null)
        {
            _recipes = (await _dataService
                .ReadFileAsync<List<RecipeData>>(_serializer, Constants.RecipeDataFile));
        }
        return _recipes ?? new List<RecipeData>();
    }

    //Implementation to update saved cookbooks and recipes in memory 
    private async ValueTask<List<SavedRecipesData>> LoadSaved()
    {
        if (_savedRecipes == null)
        {
            _savedRecipes = (await _dataService
                .ReadFileAsync<List<SavedRecipesData>>(_serializer, Constants.SavedRecipesDataFile));
        }
        return _savedRecipes ?? new List<SavedRecipesData>();
    }

    //Implementation to update saved cookbooks and recipes in memory 
    private async ValueTask<IImmutableList<CategoryData>> LoadCategories()
    {
        if (_categories == null)
        {
            _categories = (await _dataService
                .ReadFileAsync<List<CategoryData>>(_serializer, Constants.CategoryDataFile))?.ToImmutableList();
        }
        return _categories ?? ImmutableList<CategoryData>.Empty;
    }
}
