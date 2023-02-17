using Chefs.Business;
using System.Collections.Immutable;
using System.Runtime.InteropServices;
using Uno.Extensions;

namespace Chefs.Presentation;

public partial class UpdateCookbookModel // DR_REV: Use Model suffix instead of ViewModel
{
    private readonly INavigator _navigator;
    private readonly IRecipeService _recipeService;
    private readonly ICookbookService _cookbookService;
    private Cookbook _cookbook;

    public UpdateCookbookModel(
		INavigator navigator,
        IRecipeService recipeService,
        ICookbookService cookbookService,
        Cookbook cookbook)
    {
        _navigator = navigator;
        _recipeService = recipeService;
        _cookbookService = cookbookService;
        _cookbook = cookbook;
    }

    public IState<Cookbook> Cookbook => State.Value(this, () => _cookbook);

    public IListState<Recipe> Recipes => ListState.Async(this, async ct =>
    {
        var cookbook = await Cookbook;

        var recipes = await _recipeService.GetSaved(ct);
        var recipesExceptCookbook = cookbook?.Recipes is null
            ? recipes
            : recipes.Select(r => {
                if (cookbook.Recipes.Any(cbR => r.Id == cbR.Id)) r = r with { Selected = true };
                return r;
            })
            .ToImmutableList();

        return recipesExceptCookbook;
    });

    public async ValueTask Done(CancellationToken ct)
    {
        var selectedRecipes = (await Recipes).Where(x => x.Selected).ToImmutableList();
        var cookbook = _cookbook;

        if (selectedRecipes is not null && selectedRecipes.Count > 0)
        {
            var response = await _cookbookService.Update(cookbook!, selectedRecipes, ct);
            await _navigator.NavigateBackWithResultAsync(this, data: response);
        }
        else
        {
            await _navigator
                .ShowMessageDialogAsync(this, content: "Please write a cookbook name and select one recipe", title: "Error");
        }
    }
}
