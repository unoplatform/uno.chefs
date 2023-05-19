﻿namespace Chefs.Presentation;

public partial class ProfileModel
{
	private readonly ICookbookService _cookbookService;
	private readonly IRecipeService _recipeService;
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

    public Cookbook EmptyCookbook => new Cookbook();

	public IFeed<User> Profile { get; }

	public IFeed<int> RecipesCount => Profile.SelectAsync((user, ct) => _recipeService.GetCount(user.Id, ct));

	public IListFeed<Cookbook> Cookbooks => _cookbookService.SavedCookbooks;

	public IListFeed<Recipe> Recipes => Profile.SelectAsync((user, ct) => _recipeService.GetByUser(user.Id, ct)).AsListFeed();

    //We kept this navigation as workaround for issue: https://github.com/unoplatform/uno.chefs/issues/103
    public async ValueTask NavigateToSettings(CancellationToken ct)
    {
        await _navigator.NavigateViewModelAsync<SettingsModel>(this, data: await Profile, cancellation: ct);
    }

    public async ValueTask GoBack(CancellationToken ct) => await _navigator.NavigateBackAsync(this, cancellation: ct);
    
    public async ValueTask NavigateToUpdateCookBook(Cookbook cookbook, CancellationToken ct)
    {
		await _navigator.NavigateBackWithResultAsync(this, data : cookbook.UpdateCookbook(), cancellation: ct);

	}
}
