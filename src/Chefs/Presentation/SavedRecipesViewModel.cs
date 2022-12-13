﻿using Chefs.Business;
using Windows.System;

namespace Chefs.Presentation;

public partial class SavedRecipesViewModel // DR_REV: Use Model suffix instead of ViewModel
{
    private readonly INavigator _navigator;
    private readonly IRecipeService _recipeService;
    private readonly ICookbookService _cookbookService;

    // DR_REV: Alignment: one indentation at most per new line
    public SavedRecipesViewModel(
		INavigator navigator, 
        IRecipeService recipeService, 
        ICookbookService cookbookService)
    {
        _navigator = navigator;
        _recipeService = recipeService;
        _cookbookService = 
        _cookbookService = cookbookService;
    }

    public IListFeed<Cookbook> Cookbooks => ListFeed<Cookbook>.Async(async ct => await _cookbookService.GetSaved(ct));

    public IListFeed<Recipe> Recipes => ListFeed<Recipe>.Async(async ct => await _recipeService.GetSaved(ct));

    // DR_REV: XAML only nav
    public async ValueTask RecipeNavigation(Recipe recipe, CancellationToken ct)
    {
        await _navigator.NavigateViewModelAsync<RecipeDetailsViewModel>(this, data: recipe, cancellation: ct);
    }

    public async ValueTask CreateCookbookNavigation(CancellationToken ct)
    {
        await _navigator.NavigateViewModelAsync<CreateCookbookViewModel>(this, cancellation: ct);
    }

    public async ValueTask CookbookNavigation(Cookbook cookbook, CancellationToken ct)
    {
        await _navigator.NavigateViewModelAsync<LiveCookingViewModel>(this, data: cookbook, cancellation: ct);
    }

    public async ValueTask CookbookDetailNavigation(Cookbook cookbook, CancellationToken ct)
    {
        await _navigator.NavigateViewModelAsync<CookbookDetailViewModel>(this, data: cookbook, cancellation: ct);
    }
}
