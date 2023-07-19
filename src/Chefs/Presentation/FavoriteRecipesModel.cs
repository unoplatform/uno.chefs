namespace Chefs.Presentation;

public partial class FavoriteRecipesModel
{
	private readonly INavigator _navigator;
	private readonly IRecipeService _recipeService;
	private readonly ICookbookService _cookbookService;

	public FavoriteRecipesModel(
		INavigator navigator,
		IRecipeService recipeService,
		ICookbookService cookbookService)
	{
		_navigator = navigator;
		_recipeService = recipeService;
		_cookbookService = cookbookService;
	}

	public IListFeed<Cookbook> SavedCookbooks => _cookbookService.SavedCookbooks;

	public IListFeed<Recipe> SavedRecipes => _recipeService.SavedRecipes;

}