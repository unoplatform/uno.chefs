using Chefs.Presentation.Extensions;

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
	
	public IListFeed<Recipe> TrendingNow => ListFeed.Async(_recipeService.GetTrending);

	public IListFeed<CategoryWithCount> Categories => ListFeed.Async(GetCategories);

	public IListFeed<Recipe> RecentlyAdded => ListFeed.Async(_recipeService.GetRecent);

	public IListFeed<User> PopularCreators => ListFeed.Async(_userService.GetPopularCreators);

	public IFeed<User> UserProfile => _userService.User;

	public async ValueTask<ImmutableList<CategoryWithCount>> GetCategories(CancellationToken ct)
	{
		var categories = await _recipeService.GetCategories(ct);
		var categoriesWithCount = new List<CategoryWithCount>();
		foreach (var category in categories)
		{
			var recipesByCategory = await _recipeService.GetByCategory((int)category!.Id!, ct);
			categoriesWithCount.Add(new CategoryWithCount(recipesByCategory.Count, category));
		}
		return categoriesWithCount.ToImmutableList();
	}

	public async ValueTask Search() =>
		await _navigator.NavigateViewModelAsync<SearchModel>(this, qualifier: Qualifiers.Separator);

	public async ValueTask ShowAll() =>
		await _navigator.NavigateViewModelAsync<SearchModel>(this, data: new SearchFilter(OrganizeCategory.Popular, null, null, null, null));

	public async ValueTask ShowAllRecentlyAdded() =>
		await _navigator.NavigateViewModelAsync<SearchModel>(this, data: new SearchFilter(OrganizeCategory.Recent, null, null, null, null));

	public async ValueTask CategorySearch(CategoryWithCount categoryWithCount) =>
		await _navigator.NavigateViewModelAsync<SearchModel>(this, data: new SearchFilter(null, null, null, null, categoryWithCount.Category));

	public async ValueTask RecipeDetails(Recipe recipe) =>
		await _navigator.NavigateViewModelAsync<RecipeDetailsModel>(this, data: recipe);

	public async ValueTask ProfileCreator(User user) =>
		await _navigator.NavigateViewModelAsync<ProfileModel>(this, data: user);

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
		await _navigator.NavigateToNotifications(this);
	}
}
