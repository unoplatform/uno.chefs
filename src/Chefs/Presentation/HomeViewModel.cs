using Chefs.Business;

namespace Chefs.Presentation;

public partial class HomeViewModel
{
    private readonly INavigator _navigator;
    private readonly IRecipeService _recipeService;

    public HomeViewModel(INavigator navigator, IRecipeService recipe)
    {
        _navigator = navigator;
        _recipeService = recipe;
    }

    public IListFeed<Recipe> TrendingNow => ListFeed.Async(_recipeService.GetTrending); 

    public IListFeed<Category> Categories => ListFeed.Async(_recipeService.GetCategories);

    private async ValueTask Notifications(CancellationToken ct)
    => await _navigator.NavigateViewModelAsync<NotificationsViewModel>(this);

    private async ValueTask Search(CancellationToken ct)
    => await _navigator.NavigateViewModelAsync<SearchViewModel>(this);

    private async ValueTask ShowAll (CancellationToken ct, Category category)
    => await _navigator.NavigateViewModelAsync<NotificationsViewModel>(this, data: category);

    private async ValueTask RecipeDetails(CancellationToken ct, Recipe recipe)
    => await _navigator.NavigateViewModelAsync<RecipeDetailsViewModel>(this, data: recipe);

    private async ValueTask Profile(CancellationToken ct, Recipe recipe)
    => await _navigator.NavigateViewModelAsync<ProfileViewModel>(this);
}
