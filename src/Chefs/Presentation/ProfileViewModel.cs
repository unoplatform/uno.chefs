
using System.Collections.Immutable;
using Chefs.Business;
using Chefs.Data;

namespace Chefs.Presentation;

public partial class ProfileViewModel
{
    private readonly ICookbookService _cookbookService;
    private readonly IRecipeService _recipeService;
    private readonly INavigator _navigator;
    private User _user;

    public ProfileViewModel(INavigator navigator,
        ICookbookService cookbookService,
        IRecipeService recipeService,
        User user)
    {
        _navigator = navigator;
        _cookbookService = cookbookService;
        _recipeService = recipeService;
        _user = user;
    }

    public IState<bool> IsMyProfile => State<bool>.Value(this, () => _user?.IsCurrent ?? false);

    public IState<User> Profile => State.Value(this, () => _user);

    public IListFeed<Cookbook> Cookbooks => ListFeed<Cookbook>.Async(async ct => await _cookbookService.GetByUser(_user.Id, ct));

    public IListFeed<Recipe> Recipes => ListFeed<Recipe>.Async(async ct => await _recipeService.GetByUser(_user.Id, ct));

    public async ValueTask Exit(CancellationToken ct) =>
        await _navigator.NavigateBackAsync(this, cancellation: ct);

    public async ValueTask SettingsNavigation(CancellationToken ct)
    {
        var result = await _navigator.GetDataAsync<SettingsViewModel, User>(this, data: _user, cancellation: ct);

        if(result is not null)
        {
            await Profile.Update(c => result, ct);
        }
    }
    
    public async ValueTask RecipeNavigation(Recipe recipe, CancellationToken ct) => await _navigator
        .NavigateViewModelAsync<RecipeDetailsViewModel>(this, data: recipe, cancellation: ct);

    public async ValueTask CookbookNavigation(Cookbook cookbook, CancellationToken ct) =>
        await _navigator.NavigateViewModelAsync<LiveCookingViewModel>(this, data: cookbook, cancellation: ct);

    public async ValueTask CookbookDetailNavigation(Cookbook cookbook, CancellationToken ct) =>
        await _navigator.GetDataAsync<CookbookDetailProfileViewModel>(this, data: cookbook, cancellation: ct);
}
