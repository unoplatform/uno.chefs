using Chefs.Business;
using Uno.Extensions.Configuration;

namespace Chefs.Presentation;

public partial class ProfileViewModel
{
    private readonly IUserService _userService;
    private readonly ICookbookService _cookbookService;
    private readonly IRecipeService _recipeService;
    private readonly INavigator _navigator;
    private User? _user;

    public ProfileViewModel(INavigator navigator,
        ICookbookService cookbookService,
        IRecipeService recipeService,
        IUserService userService,
        User user)
    {
        _navigator = navigator;
        _cookbookService = cookbookService;
        _recipeService = recipeService;
        _userService = userService;
        _user = user;
    }

    IState<bool> _myProfile => State<bool>.Value(this, () => _user is null);

    [Value]
    IFeed<bool> IsMyProfile => _myProfile;

    IFeed<User> Profile => Feed<User>.Async(async ct => _user ?? await _userService.GetUser(ct));

    IListFeed<Cookbook> Cookbooks => ListFeed<Cookbook>.Async(async ct => await _cookbookService.GetSaved(ct));

    IListFeed<Recipe> Recipes => ListFeed<Recipe>.Async(async ct => await _recipeService.GetSaved(ct));

    public async ValueTask DoExist(CancellationToken ct)
    {
        await _navigator.NavigateBackAsync(ct);
    }

    public async ValueTask DoSettingsNavigation(CancellationToken ct)
    {
        await _navigator.NavigateViewModelAsync<SettingsViewModel>(this, cancellation: ct);
    }

    public async ValueTask DoRecipeNavigation(Recipe recipe, CancellationToken ct)
    {
        await _navigator.NavigateViewModelAsync<RecipeDetailsViewModel>(this, data: recipe, cancellation: ct);
    }

    public async ValueTask DoCookbookNavigation(Cookbook cookbook, CancellationToken ct)
    {
        await _navigator.NavigateViewModelAsync<LiveCookingViewModel>(this, data: cookbook, cancellation: ct);
    }
}
