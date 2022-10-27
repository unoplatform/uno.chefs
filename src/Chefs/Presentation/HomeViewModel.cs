using Chefs.Business;
using System.Collections.Immutable;

namespace Chefs.Presentation;

public partial class HomeViewModel
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

    public IListFeed<Recipe> RecentlyAdded => Recipes.Where(x => x.Date > DateTime.Now.AddDays(-7));

    public IListFeed<Recipe> LunchRecipes => Recipes.Where(x => x.Category.Name == "Lunch");
    public IListFeed<Recipe> DinnerRecipes => Recipes.Where(x => x.Category.Name == "Dinner");
    public IListFeed<Recipe> Snack => Recipes.Where(x => x.Category.Name == "Snack");

    public IListFeed<User> PopularCreators => ListFeed.Async(_userService.GetPopularCreators);

    private async ValueTask Notifications(CancellationToken ct)
    => await _navigator.NavigateViewModelAsync<NotificationsViewModel>(this);

    private async ValueTask Search(CancellationToken ct)
    => await _navigator.NavigateViewModelAsync<SearchViewModel>(this);

    private async ValueTask ShowAll (CancellationToken ct, SearchFilter filter)
    => await _navigator.NavigateViewModelAsync<SearchViewModel>(this, data: filter);

    private async ValueTask RecipeDetails(CancellationToken ct, Recipe recipe)
    => await _navigator.NavigateViewModelAsync<RecipeDetailsViewModel>(this, data: recipe);

    private async ValueTask Profile(CancellationToken ct, Recipe recipe)
    => await _navigator.NavigateViewModelAsync<ProfileViewModel>(this);
}
