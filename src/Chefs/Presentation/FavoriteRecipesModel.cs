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

	public async Task ShowCurrentProfile()
	{
		var response = await _navigator.NavigateRouteForResultAsync<IChefEntity>(this, "Profile");
		var result = await response!.Result;

		await (result.SomeOrDefault() switch
		{
			UpdateCookbook updateCookbook => _navigator.NavigateViewModelAsync<CreateUpdateCookbookModel>(this, data: updateCookbook.Cookbook),
			Cookbook cookbook when cookbook.Id == Guid.Empty => _navigator.NavigateViewModelAsync<CreateUpdateCookbookModel>(this),
			Cookbook cookbook => _navigator.NavigateViewModelAsync<CookbookDetailModel>(this, data: cookbook),
			object obj when obj is not null && obj.GetType() != typeof(object) => _navigator.NavigateDataAsync(this, obj),
			_ => Task.CompletedTask,
		});
	}

	public async Task ShowNotifications()
	{
		_ = _navigator.NavigateViewAsync<NotificationsContentPage>(this, qualifier: Qualifiers.Dialog);
	}
}