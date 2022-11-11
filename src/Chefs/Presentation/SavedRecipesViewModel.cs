﻿using Chefs.Business;
using Windows.System;

namespace Chefs.Presentation;

public partial class SavedRecipesViewModel
{
    private readonly INavigator _navigator;
    private readonly IRecipeService _recipeService;
    private readonly ICookbookService _cookbookService;

    public SavedRecipesViewModel(INavigator navigator, 
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
}
