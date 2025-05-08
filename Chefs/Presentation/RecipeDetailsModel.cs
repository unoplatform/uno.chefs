using System.Runtime.InteropServices;
using System.Text;
using Chefs.Services.Sharing;
using Uno.Extensions.Reactive;
using Windows.ApplicationModel.DataTransfer;
using WinRT;
using WinRT.Interop;

namespace Chefs.Presentation;

public partial record RecipeDetailsModel
{
	private readonly INavigator _navigator;
	private readonly IRecipeService _recipeService;
	private readonly IUserService _userService;
	private readonly IMessenger _messenger;
	private readonly IShareService _shareService;
	private readonly RecipeFeedProvider _recipeFeed;

	public RecipeDetailsModel(
		Recipe recipe,
		INavigator navigator,
		IRecipeService recipeService,
		IUserService userService,
		IMessenger messenger,
		IShareService shareService)
	{
		_navigator = navigator;
		_recipeService = recipeService;
		_userService = userService;
		_messenger = messenger;
		_shareService = shareService;

		Recipe = recipe;
		_recipeFeed = new(recipe, _recipeService, _userService);
	}

	public Recipe Recipe { get; }

	public IFeed<RecipeInfo> RecipeDetails => _recipeFeed.Feed;

	public async ValueTask Like(Review review, CancellationToken ct) =>
		await _recipeService.LikeReview(review, ct);

	public async ValueTask Dislike(Review review, CancellationToken ct) =>
		await _recipeService.DislikeReview(review, ct);

	public async ValueTask LiveCooking(Recipe recipe, IImmutableList<Step> steps) =>
		await _navigator.NavigateDataAsync(this, data: new LiveCookingParameter(recipe, steps));

	public async ValueTask Favorite(Recipe recipe, CancellationToken ct) 
		=> await _recipeService.Favorite(recipe, ct);

	public async Task Share(Recipe recipe, IImmutableList<Step> steps, CancellationToken ct) 
		=> await _shareService.ShareRecipe(recipe, steps, ct);

	private class RecipeFeedProvider(Recipe recipe, IRecipeService recipeService, IUserService userService)
	{
		public IFeed<RecipeInfo> Feed => Uno.Extensions.Reactive.Feed.Combine(_recipe, _user, _ingredients, _steps, _reviews)
			.Select(ToRecipeInfo);

		private IFeed<Recipe> _recipe => State.Value(this, () => recipe);

		private IFeed<User> _user => _recipe.SelectAsync(async (r, ct) => await userService.GetById(r.UserId, ct));

		private IFeed<IImmutableList<Ingredient>> _ingredients => _recipe.SelectAsync(async (r, ct) => await recipeService.GetIngredients(r.Id, ct));

		private IFeed<IImmutableList<Step>> _steps => _recipe.SelectAsync(async (r, ct) => await recipeService.GetSteps(r.Id, ct));

		private IFeed<IImmutableList<Review>> _reviews => _recipe.SelectAsync(async (r, ct) => await recipeService.GetReviews(r.Id, ct));

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
