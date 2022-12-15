using System.Collections.Immutable;
using Chefs.Business;
using Microsoft.UI.Xaml.Controls;
using Uno.Extensions.Navigation;

namespace Chefs.Presentation;

public partial class RecipeDetailsModel // DR_REV: Use Model suffix instead of ViewModel
{
    private readonly INavigator _navigator;
    private readonly IRecipeService _recipeService;
    private readonly IUserService _userService;
    private readonly Recipe _recipe;
    private readonly Signal _refresh = new();

    public RecipeDetailsModel(Recipe recipe, INavigator navigator, IRecipeService recipeService, IUserService userService)
    {
        _navigator = navigator;
        _recipeService = recipeService;
        _userService = userService;

        _recipe = recipe;
    }

	// DR_REV: This seems to be used only to display the snapshot received in ctor. There is no need to wrap it into a State, use a simple property instead:
	//          public Recipe Recipe { get; }
	public IState<Recipe> Recipe => State.Value(this, () => _recipe);
    public IState<User> User => State.Async(this, async ct => await _userService.GetById(_recipe.UserId, ct));

    public IListFeed<Ingredient> Ingredients => ListFeed.Async(async ct => await _recipeService.GetIngredients(_recipe.Id, ct));
    public IListFeed<Review> Reviews => ListFeed.Async(async ct => await _recipeService.GetReviews(_recipe.Id, ct), _refresh);
    public IListFeed<Step> Steps => ListFeed.Async(async ct => await _recipeService.GetSteps(_recipe.Id, ct));

    // DR_REV: Use implicit parameters
    public async ValueTask LiveCooking(IImmutableList<Step> steps, CancellationToken ct) =>
        await _navigator.NavigateViewModelAsync<LiveCookingModel>(this, data: new LiveCookingParameter((await Recipe)!, steps));

	// DR_REV: Use implicit parameters
	public async ValueTask IngredientsNavigation(CancellationToken ct) =>
        await _navigator.NavigateViewModelAsync<IngredientsModel>(this, data: new IngredientsParameter((await Recipe)!, await Ingredients, await Steps));

	// DR_REV: Use implicit parameters
	public async ValueTask Review(IImmutableList<Review> reviews, CancellationToken ct)
    {
        _ = await _navigator.GetDataAsync<ReviewsModel>(this, data: new ReviewParameter((await Recipe)?.Id ?? Guid.Empty, reviews));
        _refresh.Raise(); // DR_REV: Use messaging instead
    }

    public async ValueTask Save(Recipe recipe, CancellationToken ct) =>
        await _recipeService.Save(recipe, ct);

    // DR_REV: NotImplementedException should be considered as a TODO, use the `NotSupportedException("reason")` instead
    public async ValueTask Share(CancellationToken ct) =>
        throw new NotImplementedException();

	// DR_REV: XAML only nav
	public async ValueTask GoBack(CancellationToken ct) =>
        await _navigator.GoBack(this);

}
