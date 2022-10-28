using Chefs.Business;
using Uno.Extensions.Navigation;

namespace Chefs.Presentation;

public partial class RecipeDetailsViewModel
{
    private INavigator _navigator;
    private IRecipeService _recipeService;

    public RecipeDetailsViewModel(Recipe recipe, INavigator navigator, IRecipeService recipeService)
    {
        _navigator = navigator;
        _recipeService = recipeService;

        Recipe = recipe;
    }

    private Recipe Recipe { get; }

    public IState<Recipe> RecipeState => State.Value(this, () => Recipe);

    public IListFeed<Review> Reviews => ListFeed.Async(async ct => await _recipeService.GetReviews(Recipe.Id, ct));
    public IListFeed<Step> Steps => ListFeed.Async(async ct => await _recipeService.GetSteps(Recipe.Id, ct));

    private async ValueTask LiveCooking(CancellationToken ct) =>
        await _navigator.NavigateViewModelAsync<LiveCookingViewModel>(this, data: Steps);

    private async ValueTask Ingredients(CancellationToken ct) =>
        await _navigator.NavigateViewModelAsync<IngredientsViewModel>(this, data: Recipe);
}
