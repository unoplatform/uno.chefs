using System.Collections.Immutable;
using Chefs.Business;
using CommunityToolkit.Mvvm.Messaging;
using Uno.Extensions.Reactive.Messaging;

namespace Chefs.Presentation;

public partial class RecipeDetailsModel
{
    private readonly INavigator _navigator;
    private readonly IRecipeService _recipeService;
    private readonly IUserService _userService;
    private readonly Signal _refresh = new();

    public RecipeDetailsModel(Recipe recipe, INavigator navigator, IRecipeService recipeService, IUserService userService, IMessenger messenger)
    {
        _navigator = navigator;
        _recipeService = recipeService;
        _userService = userService;

        Recipe = recipe;
        messenger.Observe(Reviews, x => x.Id);
    }

    public Recipe Recipe { get; }
    public IState<User> User => State.Async(this, async ct => await _userService.GetById(Recipe.UserId, ct));
    public IListFeed<Ingredient> Ingredients => ListFeed.Async(async ct => await _recipeService.GetIngredients(Recipe.Id, ct));
    public IListState<Review> Reviews => ListState.Async(this, async ct => await _recipeService.GetReviews(Recipe.Id, ct), _refresh);
    public IListFeed<Step> Steps => ListFeed.Async(async ct => await _recipeService.GetSteps(Recipe.Id, ct));
    
    public async ValueTask Like(Review review, CancellationToken ct)
    {
        await _recipeService.LikeReview(review, ct);
        _refresh.Raise();
    }

    public async ValueTask Dislike(Review review, CancellationToken ct)
    {
        await _recipeService.DislikeReview(review, ct);
        _refresh.Raise();
    }

    public async ValueTask LiveCooking(IImmutableList<Step> steps, CancellationToken ct) =>
        await _navigator.NavigateViewModelAsync<LiveCookingModel>(this, data: new LiveCookingParameter(Recipe, steps));

	public async ValueTask IngredientsNavigation(CancellationToken ct) =>
        await _navigator.NavigateViewModelAsync<IngredientsModel>(this, data: new IngredientsParameter(Recipe, await Ingredients, await Steps));

	public async ValueTask Review(IImmutableList<Review> reviews, CancellationToken ct) =>
        await _navigator.NavigateViewModelAsync<ReviewsModel>(this, data: new ReviewParameter(Recipe.Id, reviews));
    

    public async ValueTask Save(Recipe recipe, CancellationToken ct) =>
        await _recipeService.Save(recipe, ct);

    public async ValueTask Share(CancellationToken ct) =>
        throw new NotSupportedException("to define");
}