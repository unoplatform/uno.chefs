using Chefs.Presentation.Extensions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Chefs.Presentation;

public partial class HomeModel
{
	private readonly INavigator _navigator;
	private readonly IRecipeService _recipeService;
	private readonly IUserService _userService;

	public HomeModel(INavigator navigator, IRecipeService recipe, IUserService userService)
	{
		_navigator = navigator;
		_recipeService = recipe;
		_userService = userService;
	}

	private IListFeed<Recipe> Recipes => ListFeed.Async(_recipeService.GetAll);

	public IListFeed<Recipe> TrendingNow => ListFeed.Async(_recipeService.GetTrending);

	public IListFeed<Category> Categories => ListFeed.Async(_recipeService.GetCategories);

	public IListFeed<Recipe> RecentlyAdded => ListFeed.Async(_recipeService.GetRecent);

	public IListFeed<User> PopularCreators => ListFeed.Async(_userService.GetPopularCreators);

	public IFeed<User> UserProfile => _userService.User;

	public async ValueTask Search(CancellationToken ct) =>
		await _navigator.NavigateViewModelAsync<SearchModel>(this, qualifier: Qualifiers.Separator);

	public async ValueTask ShowAll(CancellationToken ct) =>
		await _navigator.NavigateViewModelAsync<SearchModel>(this, data: new SearchFilter(OrganizeCategory.Popular, null, null, null, null));

	public async ValueTask ShowAllRecentlyAdded(CancellationToken ct) =>
		await _navigator.NavigateViewModelAsync<SearchModel>(this, data: new SearchFilter(OrganizeCategory.Recent, null, null, null, null));

	public async ValueTask CategorySearch(Category category, CancellationToken ct) =>
		await _navigator.NavigateViewModelAsync<SearchModel>(this, data: new SearchFilter(null, null, null, null, category));

	public async ValueTask RecipeDetails(Recipe recipe, CancellationToken ct) =>
		await _navigator.NavigateViewModelAsync<RecipeDetailsModel>(this, data: recipe);

	public async ValueTask ProfileCreator(User user, CancellationToken ct) =>
		await _navigator.NavigateViewModelAsync<ProfileModel>(this, data: user, cancellation: ct);

	public async ValueTask SaveRecipe(Recipe recipe, CancellationToken ct) =>
		await _recipeService.Save(recipe, ct);

	public async ValueTask ShowProfile(User profile)
	{
		await _navigator.NavigateToProfile(this, profile);
	}

	public async ValueTask ShowCurrentProfile()
	{
		await _navigator.NavigateToProfile(this);
	}

	public async ValueTask ShowNotifications()
	{
		_ = _navigator.NavigateRouteAsync(this, "Notifications", qualifier: Qualifiers.Dialog);
	}
}
