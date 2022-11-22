using Chefs.Business;
using System.Collections.Immutable;

namespace Chefs.Presentation;

public partial class CreateCookbookViewModel
{
    private readonly INavigator _navigator;
    private readonly IRecipeService _recipeService;
    private readonly ICookbookService _cookbookService;

    public CreateCookbookViewModel(INavigator navigator, 
                                   IRecipeService recipeService,
                                   ICookbookService cookbookService) 
    {
        _navigator = navigator;
        _recipeService = recipeService;
        _cookbookService = cookbookService;
    }

    public IState<string> CookbookName =>  State<string>.Empty(this);

    public IListState<Recipe> Recipes => ListState.Async(this, _recipeService.GetSaved);

    public async ValueTask Exit(CancellationToken ct) =>
       await _navigator.NavigateBackAsync(this, cancellation: ct);

    public async ValueTask Done(CancellationToken ct)
    {
        var cookbookName = await CookbookName;

        var selectedRecipes = (await Recipes).Where(x => x.Selected);
        if (!string.IsNullOrEmpty(cookbookName)
            && selectedRecipes is not null)
        {
            await _cookbookService.Create(cookbookName, selectedRecipes.ToImmutableList(), ct);
            await _navigator.NavigateBackAsync(this);
        } 
        else
        {
            await _navigator
                .ShowMessageDialogAsync(this, content: "Please write a cookbook name and select one recipe", title: "Error");
        }
    }
}
