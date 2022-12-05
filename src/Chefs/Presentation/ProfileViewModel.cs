﻿
using System.Collections.Immutable;
using Chefs.Business;
using Chefs.Data;

namespace Chefs.Presentation;

public partial class ProfileViewModel
{
    private readonly ICookbookService _cookbookService;
    private readonly IRecipeService _recipeService;
    private readonly INavigator _navigator;
    private readonly Signal _pageRefresh = new();

    public ProfileViewModel(INavigator navigator,
        ICookbookService cookbookService,
        IRecipeService recipeService,
        IUserService userService,
        User user)
    {
        _navigator = navigator;
        _cookbookService = cookbookService;
        _recipeService = recipeService;
        Profile = State.Async(this, async ct => await userService.GetCurrent(ct) ?? user);
    }

    public IState<User> Profile { get; }

    public IState<bool> IsMyProfile => State<bool>.Async(this, async ct => Profile?.Value(ct).Result!.IsCurrent ?? false, _pageRefresh);

    public IListFeed<Cookbook> Cookbooks => ListFeed<Cookbook>.Async(async ct => await _cookbookService.GetByUser((Guid)(Profile?.Value(ct).Result!.Id)!, ct), _pageRefresh);

    public IListFeed<Recipe> Recipes => ListFeed<Recipe>.Async(async ct => await _recipeService.GetByUser((Guid)(Profile?.Value(ct).Result!.Id)!, ct), _pageRefresh);

    public async ValueTask SettingsNavigation(CancellationToken ct) => await _navigator
        .NavigateViewModelAsync<SettingsViewModel>(sender: this, qualifier: "!", data: await Profile, cancellation: ct);
    
    public async ValueTask RecipeNavigation(Recipe recipe, CancellationToken ct) => await _navigator
        .NavigateViewModelAsync<RecipeDetailsViewModel>(this, data: recipe, cancellation: ct);

    public async ValueTask CookbookNavigation(Cookbook cookbook, CancellationToken ct) =>
        await _navigator.NavigateViewModelAsync<LiveCookingViewModel>(this, data: cookbook, cancellation: ct);

    public async ValueTask CookbookDetailNavigation(Cookbook cookbook, CancellationToken ct) =>
        await _navigator.NavigateViewModelAsync<CookbookDetailViewModel>(this, data: cookbook, cancellation: ct);
}
