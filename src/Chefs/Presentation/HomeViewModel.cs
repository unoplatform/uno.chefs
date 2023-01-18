﻿using Chefs.Business;
using Chefs.Data;
using System.Collections.Immutable;

namespace Chefs.Presentation;

public partial class HomeViewModel
{
    private readonly INavigator _navigator;
    private readonly IRecipeService _recipeService;
    private readonly IUserService _userService;

    public HomeViewModel(INavigator navigator, IRecipeService recipe, IUserService userService)
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

    public async ValueTask Notifications(CancellationToken ct) => 
        await _navigator.NavigateViewModelAsync<NotificationsViewModel>(this);

    public async ValueTask Search(CancellationToken ct) => 
        await _navigator.NavigateViewModelAsync<SearchViewModel>(this, qualifier: Qualifiers.Separator);

    public async ValueTask ShowAll(CancellationToken ct)
    {
        var filter = new SearchFilter(null, null, null, null, null);
        await _navigator.NavigateViewModelAsync<SearchViewModel>(this, data: filter);
    }

    public async ValueTask ShowAllRecentlyAdded(CancellationToken ct)
    {
        var filter = new SearchFilter(OrganizeCategories.Recent, null, null, null, null);
        await _navigator.NavigateViewModelAsync<SearchViewModel>(this, data: filter);
    }

    public async ValueTask ShowAllLunch(CancellationToken ct)
    {
        var filter = new SearchFilter(null, null, null, null, (await Categories.Where(x=> x.Name == "Lunch")).FirstOrDefault());
        await _navigator.NavigateViewModelAsync<SearchViewModel>(this, data: filter);
    }

    public async ValueTask ShowAllDinner(CancellationToken ct)
    {
        var filter = new SearchFilter(null, null, null, null, (await Categories.Where(x => x.Name == "Dinner")).FirstOrDefault());
        await _navigator.NavigateViewModelAsync<SearchViewModel>(this, data: filter);
    }

    public async ValueTask ShowAllSnack(CancellationToken ct)
    {
        var filter = new SearchFilter(null, null, null, null, (await Categories.Where(x => x.Name == "Dinner")).FirstOrDefault());
        await _navigator.NavigateViewModelAsync<SearchViewModel>(this, data: filter);
    }

    public async ValueTask CategorySearch(Category category, CancellationToken ct)
    {
        var filter = new SearchFilter(null, null, null, null, category);
        await _navigator.NavigateViewModelAsync<SearchViewModel>(this, data: filter);
    }

    public async ValueTask RecipeDetails(Recipe recipe, CancellationToken ct) =>
        await _navigator.NavigateViewModelAsync<RecipeDetailsViewModel>(this, data: recipe);

    public async ValueTask ProfileCreator(User user, CancellationToken ct) =>
        await _navigator.NavigateViewModelAsync<ProfileViewModel>(this, data: user, cancellation: ct);

    public async ValueTask SaveRecipe(Recipe recipe, CancellationToken ct) =>
        await _recipeService.Save(recipe, ct);
}
