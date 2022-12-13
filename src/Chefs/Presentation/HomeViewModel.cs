using Chefs.Business;
using Chefs.Data;
using System.Collections.Immutable;

namespace Chefs.Presentation;

public partial class HomeViewModel // DR_REV: Use Model suffix instead of ViewModel
{
    private readonly INavigator _navigator;
    private readonly IRecipeService _recipeService;
    private readonly IUserService _userService;

    public HomeViewModel(INavigator navigator, IRecipeService recipe, IUserService userService)
    {
        _navigator = navigator;
        _recipeService = recipe;
        _userService = userService;
    }

    private IListFeed<Recipe> Recipes => ListFeed.Async(_recipeService.GetAll);

    public IListFeed<Recipe> TrendingNow => ListFeed.Async(_recipeService.GetTrending); 

    public IListFeed<Category> Categories => ListFeed.Async(_recipeService.GetCategories);
    public IListFeed<Recipe> RecentlyAdded => ListFeed.Async(_recipeService.GetRecent);

    public IListFeed<Recipe> LunchRecipes => Recipes.Where(x => x.Category.Name == "Lunch");
    public IListFeed<Recipe> DinnerRecipes => Recipes.Where(x => x.Category.Name == "Dinner");
    public IListFeed<Recipe> SnackRecipes => Recipes.Where(x => x.Category.Name == "Snack");

    public IListFeed<User> PopularCreators => ListFeed.Async(_userService.GetPopularCreators);

    public IFeed<User> UserProfile => Feed<User>.Async(async ct => await _userService.GetCurrent(ct));

	// DR_REV: XAML only nav
	public async ValueTask Notifications(CancellationToken ct) => 
        await _navigator.NavigateViewModelAsync<NotificationsViewModel>(this);

    public async ValueTask Search(CancellationToken ct) => 
        await _navigator.NavigateViewModelAsync<SearchViewModel>(this, qualifier: Qualifiers.Separator);

    public async ValueTask ShowAll(CancellationToken ct)
    {
        var filter = new SearchFilter(null, null, null, null, null);
        await _navigator.NavigateViewModelAsync<SearchViewModel>(this, data: filter);
    }

    public async ValueTask ShowAllRecentlyAdded(CancellationToken ct)
    {
        var filter = new SearchFilter(OrganizeCategories.Recent, null, null, null, null);
        await _navigator.NavigateViewModelAsync<SearchViewModel>(this, data: filter);
    }

    // DR_REV: Use implicit parameters
    public async ValueTask ShowAllLunch(IImmutableList<Category> categories, CancellationToken ct)
    {
        var filter = new SearchFilter(null, null, null, null, categories.FirstOrDefault(x => x.Name == "Lunch"));
        await _navigator.NavigateViewModelAsync<SearchViewModel>(this, data: filter);
    }

    public async ValueTask ShowAllDinner(IImmutableList<Category> categories, CancellationToken ct)
    {
        var filter = new SearchFilter(null, null, null, null, categories.FirstOrDefault(x => x.Name == "Dinner"));
        await _navigator.NavigateViewModelAsync<SearchViewModel>(this, data: filter);
    }

    public async ValueTask ShowAllSnack(IImmutableList<Category> categories, CancellationToken ct)
    {
        var filter = new SearchFilter(null, null, null, null, categories.FirstOrDefault(x => x.Name == "Snack"));
        await _navigator.NavigateViewModelAsync<SearchViewModel>(this, data: filter);
    }

    public async ValueTask CategorySearch(Category category, CancellationToken ct)
    {
        var filter = new SearchFilter(null, null, null, null, category);
        await _navigator.NavigateViewModelAsync<SearchViewModel>(this, data: filter);
    }

    public async ValueTask RecipeDetails(Recipe recipe, CancellationToken ct) =>
        await _navigator.NavigateViewModelAsync<RecipeDetailsViewModel>(this, data: recipe);

    public async ValueTask ProfileCreator(User user, CancellationToken ct) =>
        await _navigator.NavigateViewModelAsync<ProfileViewModel>(this, data: user, cancellation: ct);

    public async ValueTask SaveRecipe(Recipe recipe, CancellationToken ct) =>
        await _recipeService.Save(recipe, ct);
}
