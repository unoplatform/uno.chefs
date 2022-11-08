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
                                   ICookbookService cookbookService) 
    {
        _navigator = navigator;
        _recipeService = recipeService;
        _cookbookService = cookbookService;

        _ = CreateCookbook(ct: CancellationToken.None);
    }

    public IState<string> CookbookName => State<string>.Empty(this);

    public IListFeed<Recipe> Recipes => ListFeed.Async(async ct => await _recipeService.GetAll(ct));

    public async ValueTask Exit(CancellationToken ct)
    {
       await _navigator.NavigateBackAsync(this, cancellation: ct);
    }

    public async ValueTask SelectedRecipient(Recipe recipe, CancellationToken ct)
    {
        var containRecipient = _cookbook is not null ? _cookbook?
            .Recipes?
            .Where(r => r.Id == recipe.Id)
            .ToList().Count > 0 : false;

        if (containRecipient)
        {
            var cookbookRecipes = _cookbook?.Recipes?.ToList();
            cookbookRecipes?.Remove(r => r.Id == recipe.Id);
            await CreateCookbook(cookbookRecipes, ct);
        }
        else
        {
            var cookbookRecipes = _cookbook?.Recipes?.ToList();
            cookbookRecipes?.Add(recipe);
            await CreateCookbook(cookbookRecipes, ct);
        }
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
            await _navigator.NavigateBackAsync(this, cancellation: ct);
        }
    }
}
