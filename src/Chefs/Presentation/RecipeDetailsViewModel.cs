using Chefs.Business;
using Microsoft.UI.Xaml.Controls;
using Uno.Extensions.Navigation;

namespace Chefs.Presentation;

public partial class RecipeDetailsViewModel
{
    private INavigator _navigator;
    private IRecipeService _recipeService;
    private readonly Recipe _recipe;

    public RecipeDetailsViewModel(Recipe recipe, INavigator navigator, IRecipeService recipeService)
    {
        _navigator = navigator;
        _recipeService = recipeService;

        _recipe = recipe;
    }

    public IState<Recipe> Recipe => State.Value(this, () => _recipe);

    public IListFeed<Review> Reviews => ListFeed.Async(async ct => await _recipeService.GetReviews(_recipe.Id, ct));
    public IListFeed<Step> Steps => ListFeed.Async(async ct => await _recipeService.GetSteps(_recipe.Id, ct));

    private async ValueTask LiveCooking(CancellationToken ct) =>
        await _navigator.NavigateViewModelAsync<LiveCookingViewModel>(this, data: Steps);

    private async ValueTask Ingredients(CancellationToken ct) =>
        await _navigator.NavigateViewModelAsync<IngredientsViewModel>(this, data: _recipe);

    private async ValueTask Review(CancellationToken ct) =>
        await _navigator.NavigateViewModelAsync<ReviewsViewModel>(this, data: _recipe);

    private async ValueTask Save(Recipe recipe, CancellationToken ct) =>
        await _recipeService.Save(recipe, ct);

    private async ValueTask Share(CancellationToken ct) =>
        throw new NotImplementedException();

    private async ValueTask GoBack(CancellationToken ct) =>
        await _navigator.GoBack(this);

}
