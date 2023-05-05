using Chefs.Business;
using Chefs.Data;
using System.Collections.Immutable;

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

    public IListFeed<Recipe> LunchRecipes => Recipes.Where(x => x.Category.Name == "Lunch");

    public IListFeed<Recipe> DinnerRecipes => Recipes.Where(x => x.Category.Name == "Dinner");

    public IListFeed<Recipe> SnackRecipes => Recipes.Where(x => x.Category.Name == "Snack");

    public IListFeed<User> PopularCreators => ListFeed.Async(_userService.GetPopularCreators);
    
    public IFeed<User> UserProfile => _userService.UserFeed;

	// TODO: DR_REV: XAML only nav - AppButtons doesnt support NavRequest yet
	public async ValueTask Notifications(CancellationToken ct) => 
        await _navigator.NavigateViewModelAsync<NotificationsModel>(this);

    public async ValueTask Search(CancellationToken ct) => 
        await _navigator.NavigateViewModelAsync<SearchModel>(this, qualifier: Qualifiers.Separator);

    public async ValueTask ShowAll(CancellationToken ct) =>
        await _navigator.NavigateViewModelAsync<SearchModel>(this, data: new SearchFilter(OrganizeCategory.Popular, null, null, null, null));
    
    public async ValueTask ShowAllRecentlyAdded(CancellationToken ct) =>
        await _navigator.NavigateViewModelAsync<SearchModel>(this, data: new SearchFilter(OrganizeCategory.Recent, null, null, null, null));

    public async ValueTask ShowAllLunch(IImmutableList<Category> categories, CancellationToken ct) =>
        await _navigator.NavigateViewModelAsync<SearchModel>(this, data: new SearchFilter(null, null, null, null, categories.FirstOrDefault(x => x.Name == "Lunch")));

    public async ValueTask ShowAllDinner(IImmutableList<Category> categories, CancellationToken ct) =>
        await _navigator.NavigateViewModelAsync<SearchModel>(this, data: new SearchFilter(null, null, null, null, categories.FirstOrDefault(x => x.Name == "Dinner")));

    public async ValueTask ShowAllSnack(IImmutableList<Category> categories, CancellationToken ct) =>
        await _navigator.NavigateViewModelAsync<SearchModel>(this, data: new SearchFilter(null, null, null, null, categories.FirstOrDefault(x => x.Name == "Snack")));
    
    public async ValueTask CategorySearch(Category category, CancellationToken ct) =>
        await _navigator.NavigateViewModelAsync<SearchModel>(this, data: new SearchFilter(null, null, null, null, category));
    
    public async ValueTask RecipeDetails(Recipe recipe, CancellationToken ct) =>
        await _navigator.NavigateViewModelAsync<RecipeDetailsModel>(this, data: recipe);

    public async ValueTask ProfileCreator(User user, CancellationToken ct) =>
        await _navigator.NavigateViewModelAsync<ProfileModel>(this, data: user, cancellation: ct);

    public async ValueTask SaveRecipe(Recipe recipe, CancellationToken ct) =>
        await _recipeService.Save(recipe, ct);

    public async Task ShowProfile(User profile)
    {
        await ProfileNavigation(profile);
    }

    public async Task ShowCurrentProfile()
    {
        await ProfileNavigation();
    }

    private async Task ProfileNavigation(User? profile = null)
    {
        var response = await _navigator.NavigateRouteForResultAsync<IChefEntity>(this, "Profile", data: profile);
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
}
