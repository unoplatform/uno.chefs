using Chefs.Business;
using System.Collections.Immutable;
using System.Runtime.InteropServices;

namespace Chefs.Presentation;

public partial class AddRecipesSavedCookbookViewModel
{
    private readonly INavigator _navigator;
    private readonly IRecipeService _recipeService;
    private readonly ICookbookService _cookbookService;

    private Cookbook? _cookbook;

    public AddRecipesSavedCookbookViewModel(INavigator navigator,
                                           IRecipeService recipeService,
                                           ICookbookService cookbookService,
                                           Cookbook cookbook)
    {
        _navigator = navigator;
        _recipeService = recipeService;
        _cookbookService = cookbookService;

        _cookbook = cookbook;
    }

    public IListFeed<Recipe> Recipes => ListFeed.Async(async ct => (await _recipeService
        .GetAll(ct)).RemoveAll(r1 => _cookbook?.Recipes!.ToList().Exists(r2 => r1.Id == r2.Id) ?? false));

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
        _cookbook = new Cookbook()
        {
            Id = _cookbook?.Id ?? Guid.NewGuid(),
            UserId = _cookbook?.UserId ?? Guid.NewGuid(),
            Name = _cookbook?.Name,
            PinsNumber = cookbookRecipes?.Count() ?? 0,
            Recipes = cookbookRecipes?.ToImmutableList() ?? (new List<Recipe>()).ToImmutableList()
        };
    }

    public async ValueTask Done(CancellationToken ct)
    {
        await _cookbookService.Create(_cookbook!, ct);
        await _navigator.NavigateBackWithResultAsync(this, data: _cookbook);
    }

    public async ValueTask Exit(CancellationToken ct)
    {
        await _navigator.NavigateBackWithResultAsync(this, data: _cookbook);
    }
}
