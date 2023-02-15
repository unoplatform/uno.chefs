using System.Collections.Immutable;
using Chefs.Business;
using Chefs.Data;

namespace Chefs.Presentation;

public partial class ProfileModel
{
    private readonly ICookbookService _cookbookService;
    private readonly IRecipeService _recipeService;
    private readonly IUserService _userService;
    private readonly INavigator _navigator;
    private readonly INavigator _sourceNavigator;

    public ProfileModel(
        NavigationRequest request,
        INavigator navigator,
        ICookbookService cookbookService,
        IRecipeService recipeService,
        IUserService userService,
        User? user)
    {
        _navigator = navigator;
        _sourceNavigator = request?.Source ?? navigator;
        _cookbookService = cookbookService;
        _recipeService = recipeService;
        Profile = user != null ? State.Value(this, () => user) : userService.UserFeed;
    }

    public IFeed<User> Profile { get; }

    public IFeed<int> RecipesCount => Profile.SelectAsync((user, ct) => _recipeService.GetCount(user.Id, ct));

    public IListFeed<Cookbook> Cookbooks => _cookbookService.SavedCookbooks;

    public IListFeed<Recipe> Recipes => Profile.SelectAsync((user, ct) => _recipeService.GetByUser(user.Id, ct)).AsListFeed();

    //We kept this navigation as workaround for issue: https://github.com/unoplatform/uno.chefs/issues/103
    public async ValueTask SettingsNavigation(CancellationToken ct) => await _sourceNavigator.NavigateRouteAsync(this, "Settings", qualifier: "!", data: await Profile, cancellation: ct);

    public async ValueTask GoBack(CancellationToken ct) => await _navigator.NavigateBackAsync(this, cancellation: ct);
    
    public async ValueTask TabletSettingsNavigation(CancellationToken ct)
    {
        await _navigator.NavigateBackAsync(this, cancellation: ct);

        //await _sourceNavigator.NavigateViewModelAsync<SettingsModel>(this, data: await Profile, cancellation: ct);
        await _sourceNavigator.NavigateRouteAsync(this, "./MainGrid/Settings", cancellation: ct);
    }
}
