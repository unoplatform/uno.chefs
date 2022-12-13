
using System.Collections.Immutable;
using Chefs.Business;
using Chefs.Data;

namespace Chefs.Presentation;

public partial class ProfileViewModel // DR_REV: Use Model suffix instead of ViewModel
{
    private readonly ICookbookService _cookbookService;
    private readonly IRecipeService _recipeService;
    private readonly INavigator _navigator;
    private readonly User _user;

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

	// DR_REV: This seems to be used only to display the snapshot received in ctor. There is no need to wrap it into a State, use a simple property instead:
	//          public bool IsMyProfile { get; }
	//      OR
	//          As this seems to be inferred from the current 'Profile', make this value a projection of the `Profile`
	//          public IFee<bool> IsMyProfile => Profile.Select(profile => profile.IsCurrent);
    //      OR
    //          For the same reason as above, remove this property and change your binding to `Profile.IsCurrent`.
	public IState<bool> IsMyProfile => State<bool>.Value(this, () => _user?.IsCurrent ?? false);

    public IState<User> Profile => State.Value(this, () => _user);

	// DR_REV: This is a projection of the current user, use async projection instead:
	public IListFeed<Cookbook> Cookbooks => Profile.SelectAsync((user, ct) => _cookbookService.GetByUser(user.Id, ct)).AsListFeed();

	// DR_REV: This is a projection of the current user, use async projection instead:
	public IListFeed<Recipe> Recipes => Profile.SelectAsync((user, ct) => _recipeService.GetByUser(user.Id, ct)).AsListFeed();

    // DR_REV: XAML only nav
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
        await _navigator.NavigateViewModelAsync<CookbookDetailViewModel>(this, data: cookbook, cancellation: ct);
}
