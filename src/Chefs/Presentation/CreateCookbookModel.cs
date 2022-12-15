using System.Collections.Generic;
using System.Collections.Immutable;
using System.Runtime.InteropServices;
using Chefs.Business;
using Uno.Extensions.Reactive;

namespace Chefs.Presentation;

public partial class CreateCookbookModel // DR_REV: Use Model suffix instead of ViewModel
{
    private readonly INavigator _navigator;
    private readonly IRecipeService _recipeService;
    private readonly ICookbookService _cookbookService;

    public CreateCookbookModel(
		INavigator navigator, 
        IRecipeService recipeService,
        ICookbookService cookbookService) 
    {
        _navigator = navigator;
        _recipeService = recipeService;
        _cookbookService = cookbookService;
    }

    public IState<string> CookbookName =>  State<string>.Value(this, () => string.Empty);

    public IListState<Recipe> Recipes => ListState.Async(this, _recipeService.GetSaved);

	// DR_REV: You should be able to use XAML nav only and remove this method
	public async ValueTask Exit(CancellationToken ct) =>
       await _navigator.NavigateBackAsync(this, cancellation: ct);

    public async ValueTask Done(IImmutableList<Recipe> recipes, CancellationToken ct)
    {
        // DR_REV: Use CookbookName = State.Value instead of Empty, then you can implicitly get the 'cookBookName' and 'recipes' parameters (matching types and names)
        var cookbookName = await CookbookName;

        var selectedRecipes = recipes.Where(x => x.Selected);
        if (!string.IsNullOrEmpty(cookbookName)
            && selectedRecipes.Any()) // DR_REV: 'selectedRecipes' cannot be null here
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
