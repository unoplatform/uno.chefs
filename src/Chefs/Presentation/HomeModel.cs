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

	public IListFeed<CategoryWithCount> Categories => ListFeed.Async(_recipeService.GetCategoriesWithCount);

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

	public async ValueTask ShowAll(CancellationToken ct) =>
		await _navigator.NavigateViewModelAsync<SearchModel>(this, data: new SearchFilter(FilterGroup: FilterGroup.Popular));

	public async ValueTask ShowAllRecentlyAdded(CancellationToken ct) =>
		await _navigator.NavigateViewModelAsync<SearchModel>(this, data: new SearchFilter(FilterGroup: FilterGroup.Recent));

	public async ValueTask CategorySearch(CategoryWithCount categoryWithCount, CancellationToken ct) =>
		await _navigator.NavigateViewModelAsync<SearchModel>(this, qualifier: Qualifiers.ClearBackStack, data: new SearchFilter(Category: categoryWithCount.Category));

	public async ValueTask SaveRecipe(Recipe recipe, CancellationToken ct) =>
		await _recipeService.Save(recipe, ct);
}
