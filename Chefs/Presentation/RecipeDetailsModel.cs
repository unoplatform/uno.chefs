using Chefs.Business.Services.Sharing;

namespace Chefs.Presentation;

public partial record RecipeDetailsModel
{
	private readonly INavigator _navigator;
	private readonly IRecipeService _recipeService;
	private readonly IUserService _userService;
	private readonly IShareService _shareService;
	private readonly RecipeFeedProvider _recipeFeed;

	public RecipeDetailsModel(
		Recipe recipe,
		INavigator navigator,
		IRecipeService recipeService,
		IUserService userService,
		IShareService shareService)
	{
		_navigator = navigator;
		_recipeService = recipeService;
		_userService = userService;
		_shareService = shareService;

		Recipe = recipe;
		_recipeFeed = new RecipeFeedProvider(recipe, _recipeService, _userService);
	}

	public Recipe Recipe { get; }

	public IFeed<RecipeInfo> RecipeDetails => _recipeFeed.Feed;

	public async ValueTask Like(Review review, CancellationToken ct)
		=> await _recipeService.LikeReview(review, ct);

	public async ValueTask Dislike(Review review, CancellationToken ct)
		=> await _recipeService.DislikeReview(review, ct);

	public async ValueTask LiveCooking(RecipeInfo recipeDetails)
		=> await _navigator.NavigateDataAsync(this, data: new LiveCookingParameter(recipeDetails.Recipe, recipeDetails.Steps));

	public async ValueTask Favorite(RecipeInfo recipeDetails, CancellationToken ct)
		=> await _recipeService.Favorite(recipeDetails.Recipe, ct);

	public async Task Share(RecipeInfo recipeDetails, CancellationToken ct)
		=> await _shareService.ShareRecipe(recipeDetails.Recipe, recipeDetails.Steps, ct);

	private class RecipeFeedProvider(Recipe recipe, IRecipeService recipeService, IUserService userService)
	{
		public IFeed<RecipeInfo> Feed => Uno.Extensions.Reactive.Feed
			.Combine(Recipe, User, Ingredients, Steps, Reviews)
			.Select(ToRecipeInfo);

		private IFeed<Recipe> Recipe => State.Value(this, () => recipe);

		private IFeed<User> User => Recipe.SelectAsync(async (r, ct) => await userService.GetById(r.UserId, ct));

		private IFeed<IImmutableList<Ingredient>> Ingredients => Recipe.SelectAsync(async (r, ct) => await recipeService.GetIngredients(r.Id, ct));

		private IFeed<IImmutableList<Step>> Steps => Recipe.SelectAsync(async (r, ct) => await recipeService.GetSteps(r.Id, ct));

		private IFeed<IImmutableList<Review>> Reviews => Recipe.SelectAsync(async (r, ct) => await recipeService.GetReviews(r.Id, ct));

		private RecipeInfo ToRecipeInfo((Recipe recipe, User user, IImmutableList<Ingredient> ingredients, IImmutableList<Step> steps, IImmutableList<Review> reviews) values)
			=> new RecipeInfo(
				values.recipe,
				values.user,
				values.steps,
				values.ingredients,
				values.reviews);
	}

	public record RecipeInfo(Recipe Recipe, User User, IImmutableList<Step> Steps, IImmutableList<Ingredient> Ingredients, IImmutableList<Review> Reviews);
}
