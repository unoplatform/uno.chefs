using Chefs.Business;

namespace Chefs.Presentation;

public partial class SavedRecipesModel
{
    private readonly INavigator _navigator;
    private readonly IRecipeService _recipeService;
    private readonly ICookbookService _cookbookService;

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

    public async ValueTask CreateCookbookNavigation(CancellationToken ct)
    {
        var result = await _navigator.GetDataAsync<CreateCookbookModel, Cookbook>(this, cancellation: ct);

        if (result is not null)
        {
            await Cookbooks.AddAsync(result, ct);
        }
    }
}