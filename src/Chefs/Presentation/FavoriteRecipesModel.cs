using Chefs.Presentation.Extensions;

namespace Chefs.Presentation;

public partial class FavoriteRecipesModel
{
	private readonly INavigator _navigator;
	private readonly IRecipeService _recipeService;
	private readonly ICookbookService _cookbookService;
	private readonly IMessenger _messenger;

	public FavoriteRecipesModel(
		INavigator navigator,
		IRecipeService recipeService,
		ICookbookService cookbookService,
		IMessenger messenger)
	{
		_navigator = navigator;
		_recipeService = recipeService;
		_cookbookService = cookbookService;
		_messenger = messenger;

		_messenger.Observe(SavedCookbooks, cb => cb.Id);
		_messenger.Observe(SavedRecipes, r => r.Id);
	}

	public IListState<Cookbook> SavedCookbooks => ListState.FromFeed(this, _cookbookService.SavedCookbooks);

	public IListState<Recipe> SavedRecipes => ListState.FromFeed(this, _recipeService.SavedRecipes);
}
