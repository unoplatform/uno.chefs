using Chefs.Business;
using System.Collections.Immutable;

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

    public IListFeed<Cookbook> Cookbooks => _cookbookService.SavedCookbooks.AsFeed().Select(cookbooks => Enumerable.Range(0, 10).Select(i => cookbooks.First() with { Name = $"Cookbook {i}", Id = Guid.NewGuid() }).ToImmutableList()).AsListFeed();

    public IListFeed<Recipe> Recipes => _recipeService.SavedRecipes;
}