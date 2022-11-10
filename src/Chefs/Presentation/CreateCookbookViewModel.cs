using System.Collections.Generic;
using System.Collections.Immutable;
using System.Runtime.InteropServices;
using Chefs.Business;
using Uno.Extensions.Reactive;

namespace Chefs.Presentation;

public partial class CreateCookbookViewModel
{
    private readonly INavigator _navigator;
    private readonly IRecipeService _recipeService;
    private readonly ICookbookService _cookbookService;

    private Cookbook? _cookbook;

    public CreateCookbookViewModel(INavigator navigator, 
                                   IRecipeService recipeService,
                                   ICookbookService cookbookService,
                                   Cookbook? cookbook) 
    {
        _navigator = navigator;
        _recipeService = recipeService;
        _cookbookService = cookbookService;

        _cookbook = cookbook;
    }

    public IState<Cookbook?> Cookbook => State.Value(this, () => _cookbook);

    public IState<string> CookbookName => _cookbook == null 
        ? State<string>.Empty(this) 
        : State.Value(this, () => _cookbook?.Name!);

    public IListFeed<Recipe> Recipes => ListFeed.Async(async ct => _cookbook == null 
        ? await _recipeService.GetAll(ct)
        : (await _recipeService
        .GetAll(ct)).RemoveAll(r1 => _cookbook?.Recipes!.ToList().Exists(r2 => r1.Id == r2.Id) 
        ?? false));

    public async ValueTask Exit(CancellationToken ct)
    {
       await _navigator.NavigateBackAsync(this, cancellation: ct);
    }

    public async ValueTask SelectedRecipient(Recipe recipe, CancellationToken ct)
    {
        if (_cookbook is null) await CreateCookbook(ct: ct);

        var containRecipient =  _cookbook?
        .Recipes?
        .Where(r => r.Id == recipe.Id)
        .ToList().Count > 0;

        var cookbookRecipes = _cookbook?.Recipes?.ToList();

        if (containRecipient) cookbookRecipes?.Remove(r => r.Id == recipe.Id);
        else cookbookRecipes?.Add(recipe);

        await CreateCookbook(cookbookRecipes, ct);
    }

    private async Task CreateCookbook([Optional] List<Recipe>? cookbookRecipes, CancellationToken ct)
    {
        var cookbookName = await CookbookName.Value(ct);
        _cookbook = new Cookbook()
        {
            Id = _cookbook?.Id ?? Guid.NewGuid(),
            UserId = _cookbook?.UserId ?? Guid.NewGuid(),
            Name = cookbookName,
            PinsNumber = cookbookRecipes?.Count() ?? 0,
            Recipes = cookbookRecipes?.ToImmutableList() ?? (new List<Recipe>()).ToImmutableList()
        };
    }

    public async ValueTask Done(CancellationToken ct)
    {
        var cookbookName = await CookbookName.Value(ct);
        if (String.IsNullOrEmpty(cookbookName) 
            || _cookbook?.PinsNumber == 0 ) 
        {
            await _navigator
                .ShowMessageDialogAsync(this, content: "Please select a cookbook name or select recipe", title: "Error");
        } 
        else
        {
            await _cookbookService.Create(_cookbook!, ct);
            await _navigator.NavigateBackWithResultAsync(this, data: _cookbook);
        }
    }
}
