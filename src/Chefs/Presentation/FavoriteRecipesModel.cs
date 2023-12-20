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

	public async ValueTask ShowNotifications()
	{
		await _navigator.NavigateToNavigations(this);
	}
}
