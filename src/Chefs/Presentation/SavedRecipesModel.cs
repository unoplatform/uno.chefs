using Chefs.Business;
using Windows.System;

namespace Chefs.Presentation;

public partial class SavedRecipesModel // DR_REV: Use Model suffix instead of ViewModel
{
    private readonly INavigator _navigator;
    private readonly IRecipeService _recipeService;
    private readonly ICookbookService _cookbookService;

    // DR_REV: Alignment: one indentation at most per new line
    public SavedRecipesModel(
		INavigator navigator, 
        IRecipeService recipeService, 
        ICookbookService cookbookService)
    {
        _navigator = navigator;
        _recipeService = recipeService;
        _cookbookService = cookbookService;
    }

    public IListState<Cookbook> Cookbooks => ListState<Cookbook>.Async(this, async ct => await _cookbookService.GetSaved(ct));

    public IListFeed<Recipe> Recipes => ListFeed<Recipe>.Async(async ct => await _recipeService.GetSaved(ct));

    // DR_REV: XAML only nav
    public async ValueTask RecipeNavigation(Recipe recipe, CancellationToken ct)
    {
        await _navigator.NavigateViewModelAsync<RecipeDetailsModel>(this, data: recipe, cancellation: ct);
    }

    public async ValueTask CreateCookbookNavigation(CancellationToken ct)
    {
        var result = await _navigator.GetDataAsync<CreateCookbookModel, Cookbook>(this, cancellation: ct);

        if (result is not null)
        {
            await Cookbooks.AddAsync(result, ct);
        }
    }

    public async ValueTask CookbookDetailNavigation(Cookbook cookbook, CancellationToken ct)
    {
        await _navigator.NavigateViewModelAsync<CookbookDetailModel>(this, data: cookbook, cancellation: ct);
    }
}
