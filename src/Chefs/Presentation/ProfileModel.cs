
using System.Collections.Immutable;
using Chefs.Business;
using Chefs.Data;

namespace Chefs.Presentation;

public partial class ProfileModel // DR_REV: Use Model suffix instead of ViewModel
{
    private readonly ICookbookService _cookbookService;
    private readonly IRecipeService _recipeService;
    private readonly INavigator _navigator;

    public ProfileModel(INavigator navigator,
        ICookbookService cookbookService,
        IRecipeService recipeService,
        IUserService userService,
        User user)
    {
        _navigator = navigator;
        _cookbookService = cookbookService;
        _recipeService = recipeService;
        Profile = State.Async(this, async ct => user ?? await userService.GetCurrent(ct));
    }

    public IState<User> Profile { get; }

	public IListFeed<Cookbook> Cookbooks => Profile.SelectAsync((user, ct) => _cookbookService.GetByUser(user.Id, ct)).AsListFeed();

	public IListFeed<Recipe> Recipes => Profile.SelectAsync((user, ct) => _recipeService.GetByUser(user.Id, ct)).AsListFeed();

    // DR_REV: XAML only nav
	public async ValueTask Exit(CancellationToken ct) =>
        await _navigator.NavigateBackAsync(this, cancellation: ct);

	public async ValueTask SettingsNavigation(CancellationToken ct)
    {
        var result = await _navigator.GetDataAsync<SettingsModel, User>(this, data: await Profile, cancellation: ct);

        if(result is not null)
        {
            await Profile.Update(c => result, ct);
        }
    }
    
    public async ValueTask RecipeNavigation(Recipe recipe, CancellationToken ct) => await _navigator
        .NavigateViewModelAsync<RecipeDetailsModel>(this, data: recipe, cancellation: ct);

    public async ValueTask CookbookNavigation(Cookbook cookbook, CancellationToken ct) =>
        await _navigator.NavigateViewModelAsync<LiveCookingModel>(this, data: cookbook, cancellation: ct);

    public async ValueTask CookbookDetailNavigation(Cookbook cookbook, CancellationToken ct) =>
        await _navigator.NavigateViewModelAsync<CookbookDetailModel>(this, data: cookbook, cancellation: ct);
}
