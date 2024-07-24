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
	}

	public IListState<Cookbook> SavedCookbooks => ListState.Async(this, _cookbookService.GetSaved);

	public IListState<Recipe> FavoriteRecipes => _recipeService.FavoritedRecipes;
}
