using Chefs.Business;
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
        _cookbookService = cookbookService;
    }

    public IListFeed<Cookbook> Cookbooks => ListFeed<Cookbook>.Async(async ct => await _cookbookService.GetSaved(ct));

    public IListFeed<Recipe> Recipes => ListFeed<Recipe>.Async(async ct => await _recipeService.GetSaved(ct));

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
