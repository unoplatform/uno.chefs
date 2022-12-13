using Chefs.Business;
using System.Collections.Immutable;
using System.Runtime.InteropServices;
using Uno.Extensions;

namespace Chefs.Presentation;

public partial class UpdateCookbookViewModel // DR_REV: Use Model suffix instead of ViewModel
{
    private readonly INavigator _navigator;
    private readonly IRecipeService _recipeService;
    private readonly ICookbookService _cookbookService;

    // DR_DEV: Alignment: at most one tab on new lines
    public UpdateCookbookViewModel(
		INavigator navigator,
        IRecipeService recipeService,
        ICookbookService cookbookService,
        Cookbook cookbook)
    {
        _navigator = navigator;
        _recipeService = recipeService;
        _cookbookService = cookbookService;

        Cookbook = State.Value(this, () => cookbook);
    }

    public IState<Cookbook> Cookbook { get; }

    public IListState<Recipe> Recipes => ListState.Async(this, async ct =>
    {
        var cookbook = await Cookbook;
        // DR_REV: Alignment: the code was miss-aligned making it are to read ... re-aligning it shows that you are doing a cookbook.Recipes.ToList() **for each** recipe

		var recipes = await _recipeService.GetSaved(ct);
		var recipesExceptCookbook = cookbook?.Recipes is null 
			? recipes
			: recipes.RemoveAll(r => cookbook.Recipes.Any(cbR => r.Id == cbR.Id));

		return recipesExceptCookbook;
    });

    // DR_REV: XAML only nav
    public async ValueTask Exit(CancellationToken ct) =>
        await _navigator.NavigateBackAsync(this, cancellation: ct);

    public async ValueTask Done(CancellationToken ct)
    {
        var selectedRecipes = (await Recipes).Where(x => x.Selected).ToImmutableList();
        var cookbook = await Cookbook;

        if (selectedRecipes is not null)
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
