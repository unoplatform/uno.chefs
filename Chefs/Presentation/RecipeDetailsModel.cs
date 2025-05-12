using System.Runtime.InteropServices;
using System.Text;
using Chefs.Services.Sharing;
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
	}

	public Recipe Recipe { get; }
	public IState<bool> IsFavorited => State.Value(this, () => Recipe.IsFavorite);

	public IState<User> User => State.Async(this, async ct => await _userService.GetById(Recipe.UserId, ct))
		.Observe(_messenger, u => u.Id);
	
	public IFeed<User> CurrentUser => Feed.Async(_userService.GetCurrent);
	public IListFeed<Ingredient> Ingredients => ListFeed.Async(async ct => await _recipeService.GetIngredients(Recipe.Id, ct));
	public IListFeed<Step> Steps => ListFeed.Async(async ct => await _recipeService.GetSteps(Recipe.Id, ct));

	public IListState<Review> Reviews => ListState
		.Async(this, async ct => await _recipeService.GetReviews(Recipe.Id, ct))
		.Observe(_messenger, r => r.Id);

	public async ValueTask Like(Review review, CancellationToken ct) =>
		await _recipeService.LikeReview(review, ct);

	public async ValueTask Dislike(Review review, CancellationToken ct) =>
		await _recipeService.DislikeReview(review, ct);

	public async ValueTask LiveCooking(IImmutableList<Step> steps) =>
		await _navigator.NavigateRouteAsync(this, "LiveCooking", data: new LiveCookingParameter(Recipe, steps));

	public async ValueTask Favorite(CancellationToken ct)
	{
		await _recipeService.Favorite(Recipe, ct);
		await IsFavorited.UpdateAsync(s => !s);
	}

	public async Task Share(CancellationToken ct)
	{
		await _shareService.ShareRecipe(Recipe, await Steps, ct);
	}
}
