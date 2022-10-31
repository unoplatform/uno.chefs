using Chefs.Business;
using Uno.Extensions.Configuration;

namespace Chefs.Presentation;

public partial class SettingsViewModel
{
    private readonly IUserService _userService;
    private readonly ICookbookService _cookbookService;
    private readonly IRecipeService _recipeService;
    private readonly INavigator _navigator;
    private User? _user;

    public SettingsViewModel(INavigator navigator,
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

    IState<bool> _myState => State<bool>.Async(this, async (ct) => _user is null);

    IState<bool> IsMyProfile => _myState;

    IState<User> Profile => State<User>.Async(this, async (ct) => _user ?? await _userService.GetUser(ct));

    IListFeed<Cookbook> Cookbooks => ListFeed<Cookbook>.Async(async ct => await _cookbookService.GetSaved(ct));

    IListFeed<Recipe> Recipes => ListFeed<Recipe>.Async(async ct => await _recipeService.GetSaved(ct));

    public async ValueTask DoExist(CancellationToken ct)
    {
        await _navigator.NavigateBackAsync(ct);
    }

    public async ValueTask DoNavigateSettigns(CancellationToken ct)
    {
        await _navigator.NavigateDataAsync<>
    }
}
