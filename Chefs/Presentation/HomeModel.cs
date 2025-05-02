namespace Chefs.Presentation;

public partial record HomeModel
{
	private readonly INavigator _navigator;
	private readonly IRecipeService _recipeService;
	private readonly IUserService _userService;
	private readonly IMessenger _messenger;

	public HomeModel(INavigator navigator, IRecipeService recipe, IUserService userService, IMessenger messenger)
	{
		_navigator = navigator;
		_recipeService = recipe;
		_userService = userService;
		_messenger = messenger;
	}
	
	public IListState<Recipe> TrendingNow => ListState
		.Async(this, _recipeService.GetTrending)
		.Observe(_messenger, r => r.Id);
	
	public IListFeed<CategoryWithCount> Categories => ListFeed.Async(_recipeService.GetCategoriesWithCount);

	public IListFeed<Recipe> RecentlyAdded => ListFeed.Async(_recipeService.GetRecent);

	public IListFeed<User> PopularCreators => ListFeed.Async(_userService.GetPopularCreators);

	public IFeed<User> UserProfile => _userService.User;

	public async ValueTask ShowAll(CancellationToken ct) =>
		await _navigator.NavigateViewModelAsync<SearchModel>(this, data: new SearchFilter(FilterGroup: FilterGroup.Popular), cancellation: ct);

	public async ValueTask ShowAllRecentlyAdded(CancellationToken ct) =>
		await _navigator.NavigateViewModelAsync<SearchModel>(this, data: new SearchFilter(FilterGroup: FilterGroup.Recent), cancellation: ct);

	public async ValueTask CategorySearch(CategoryWithCount categoryWithCount, CancellationToken ct) =>
		await _navigator.NavigateViewModelAsync<SearchModel>(this, qualifier: Qualifiers.ClearBackStack, data: new SearchFilter(Category: categoryWithCount.Category), cancellation: ct);

	public async ValueTask FavoriteRecipe(Recipe recipe, CancellationToken ct) =>
		await _recipeService.Favorite(recipe, ct);
}
