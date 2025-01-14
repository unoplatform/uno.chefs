namespace Chefs.Presentation;

public partial record FavoriteRecipesModel
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
	}
	
	public IListState<Cookbook> SavedCookbooks => ListState
		.Async(this, _cookbookService.GetSaved)
		.Observe(_messenger, cb => cb.Id);
	
	public IListState<Recipe> FavoriteRecipes => _recipeService.FavoritedRecipes;

	public async ValueTask TestCommand()
	{
		await _navigator.NavigateRouteAsync(this, "CreateCookbook");
	}
}
