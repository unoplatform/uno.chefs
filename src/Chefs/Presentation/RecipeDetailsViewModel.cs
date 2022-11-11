﻿using Chefs.Business;
using Microsoft.UI.Xaml.Controls;
using Uno.Extensions.Navigation;

namespace Chefs.Presentation;

public partial class RecipeDetailsViewModel
{
    private INavigator _navigator;
    private IRecipeService _recipeService;
    private IUserService _userService;
    private readonly Recipe _recipe;

    public RecipeDetailsViewModel(Recipe recipe, INavigator navigator, IRecipeService recipeService, IUserService userService)
    {
        _navigator = navigator;
        _recipeService = recipeService;
        _userService = userService;

        _recipe = recipe;
    }

    public IState<Recipe> Recipe => State.Value(this, () => _recipe);
    public IState<User> User => State.Async(this, async ct => await _userService.GetById(_recipe.Id, ct));

    public IListFeed<Ingredient> Ingredients => ListFeed.Async(async ct => await _recipeService.GetIngredients(_recipe.Id, ct));
    public IListFeed<Review> Reviews => ListFeed.Async(async ct => await _recipeService.GetReviews(_recipe.Id, ct));
    public IListFeed<Step> Steps => ListFeed.Async(async ct => await _recipeService.GetSteps(_recipe.Id, ct));

    public async ValueTask LiveCooking(CancellationToken ct) =>
        await _navigator.NavigateViewModelAsync<LiveCookingViewModel>(this, data: await Steps);
    
    public async ValueTask IngredientsNavigation(CancellationToken ct) =>
        await _navigator.NavigateViewModelAsync<IngredientsViewModel>(this, data: await Ingredients);

    public async ValueTask Review(CancellationToken ct) =>
        await _navigator.NavigateViewModelAsync<ReviewsViewModel>(this, data: new ReviewParameter((await Recipe)?.Id ?? Guid.Empty, await Reviews));

    public async ValueTask Save(Recipe recipe, CancellationToken ct) =>
        await _recipeService.Save(recipe, ct);

    public async ValueTask Share(CancellationToken ct) =>
        throw new NotImplementedException();

    public async ValueTask GoBack(CancellationToken ct) =>
        await _navigator.GoBack(this);

}
