using Chefs.Business;
using System.Collections.Immutable;
using System.Runtime.InteropServices;
using Uno.Extensions;

namespace Chefs.Presentation;

public partial class UpdateCookbookViewModel
{
    private readonly INavigator _navigator;
    private readonly IRecipeService _recipeService;
    private readonly ICookbookService _cookbookService;

    public UpdateCookbookViewModel(INavigator navigator,
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

    public IListFeed<Recipe> Recipes => ListFeed.Async(async ct =>
    {
        var cookbook = await Cookbook;
        return (await _recipeService.GetSaved(ct))
        .RemoveAll(r1 => cookbook?.Recipes!.ToList()
        .Exists(r2 => r1.Id == r2.Id) ?? false);
    });

    public async ValueTask Exit(CancellationToken ct) =>
        await _navigator.NavigateBackAsync(this, cancellation: ct);
    

    public async ValueTask SelectedRecipient(Recipe recipe, CancellationToken ct)
    {
        var cookbook = await Cookbook;

        var containRecipient = cookbook?
        .Recipes?
        .Where(r => r.Id == recipe.Id)
        .ToList().Count > 0;

        var cookbookRecipes = cookbook?.Recipes?.ToList();

        if (containRecipient) cookbookRecipes?.Remove(r => r.Id == recipe.Id);
        else cookbookRecipes?.Add(recipe);
    }

    public async ValueTask Done(CancellationToken ct)
    {
        var cookbook = await Cookbook;

        if (String.IsNullOrEmpty(cookbook?.Name)
            || cookbook?.PinsNumber == 0)
        {
            await _navigator
                .ShowMessageDialogAsync(this, content: "Please select a cookbook name or select recipe", title: "Error");
        }
        else
        {
            await _cookbookService.Update(cookbook!, ct);
            await _navigator.NavigateBackWithResultAsync(this, data: cookbook);
        }
    }
}
