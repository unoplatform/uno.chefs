namespace Chefs.Presentation;

public partial class RecipeDetailsModel
{
	private readonly INavigator _navigator;
	private readonly IRecipeService _recipeService;
	private readonly IUserService _userService;
	private readonly IMessenger _messenger;

	public RecipeDetailsModel(Recipe recipe, INavigator navigator, IRecipeService recipeService, IUserService userService, IMessenger messenger)
	{
		_navigator = navigator;
		_recipeService = recipeService;
		_userService = userService;

		Recipe = recipe;
		_messenger = messenger;
		messenger.Observe(Reviews, x => x.Id);
	}

	public Recipe Recipe { get; }
	public IState<User> User => State.Async(this, async ct => await _userService.GetById(Recipe.UserId, ct));
	public IFeed<User> CurrentUser => Feed.Async(async ct => await _userService.GetCurrent(ct));
	public IListFeed<Ingredient> Ingredients => ListFeed.Async(async ct => await _recipeService.GetIngredients(Recipe.Id, ct));
	public IListState<Review> Reviews => ListState.Async(this, async ct => await _recipeService.GetReviews(Recipe.Id, ct));
	public IListFeed<Step> Steps => ListFeed.Async(async ct => await _recipeService.GetSteps(Recipe.Id, ct));

	public async ValueTask Like(Review review, CancellationToken ct)
	{
		await _recipeService.LikeReview(review, ct);
	}

	public async ValueTask Dislike(Review review, CancellationToken ct)
	{
		await _recipeService.DislikeReview(review, ct);
	}

	public async ValueTask LiveCooking(IImmutableList<Step> steps, CancellationToken ct)
	{
		var route = _navigator?.Route?.Base switch
		{
			"RecipeDetails" => "LiveCooking",
			"SearchRecipeDetails" => "SearchLiveCooking",
			"FavoriteRecipeDetails" => "FavoriteLiveCooking",
			"CookbookRecipeDetails" => "CookbookLiveCooking",
			_ => throw new InvalidOperationException("Navigating from unknown route")
		};

		await _navigator.NavigateRouteAsync(this, route, data: new LiveCookingParameter(Recipe, steps), cancellation: ct);
	}

	public async ValueTask Review(IImmutableList<Review> reviews, CancellationToken ct) =>
		await _navigator.NavigateRouteAsync(this, "Reviews", data: new ReviewParameter(Recipe.Id, reviews), qualifier: Qualifiers.Dialog, cancellation: ct);

	public async ValueTask Save(Recipe recipe, CancellationToken ct) =>
		await _recipeService.Save(recipe, ct);

	public async ValueTask Share(CancellationToken ct) =>
		throw new NotSupportedException("to define");
}
