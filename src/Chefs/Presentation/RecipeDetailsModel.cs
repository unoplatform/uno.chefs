using System.Runtime.InteropServices;
using System.Text;
using Windows.ApplicationModel.DataTransfer;
using WinRT;
using WinRT.Interop;

namespace Chefs.Presentation;

public partial class RecipeDetailsModel
{
	private static readonly Guid _dtm_iid = new Guid("a5caee9b-8708-49d1-8d36-67d25a8da00c");

#if WINDOWS
	static IDataTransferManagerInterop DataTransferManagerInterop => DataTransferManager.As<IDataTransferManagerInterop>();
#endif

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

	public async ValueTask Like(Review review, CancellationToken ct) =>
		await _recipeService.LikeReview(review, ct);

	public async ValueTask Dislike(Review review, CancellationToken ct) =>
		await _recipeService.DislikeReview(review, ct);

	public async ValueTask LiveCooking(IImmutableList<Step> steps)
	{
		var route = _navigator?.Route?.Base switch
		{
			"RecipeDetails" => "LiveCooking",
			"SearchRecipeDetails" => "SearchLiveCooking",
			"FavoriteRecipeDetails" => "FavoriteLiveCooking",
			"CookbookRecipeDetails" => "CookbookLiveCooking",
			_ => throw new InvalidOperationException("Navigating from unknown route")
		};

		await _navigator.NavigateRouteAsync(this, route, data: new LiveCookingParameter(Recipe, steps));
	}

	public async ValueTask Save(CancellationToken ct) => 
		await _recipeService.Save(Recipe, ct);

	public async ValueTask Share(CancellationToken ct)
	{
#if WINDOWS
		IntPtr result;
		var hwnd = WindowNative.GetWindowHandle(App.MainWindow); 
		result = DataTransferManagerInterop.GetForWindow(hwnd, _dtm_iid);
		DataTransferManager dataTransferManager = MarshalInterface<DataTransferManager>.FromAbi(result);
		dataTransferManager.DataRequested += DataRequested;
		DataTransferManagerInterop.ShowShareUIForWindow(hwnd, null);
#else
		var dataTransferManager = DataTransferManager.GetForCurrentView();
        dataTransferManager.DataRequested += DataRequested;
        DataTransferManager.ShowShareUI();
#endif
	}

	private async void DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
	{
		args.Request.Data.Properties.Title = $"Sharing {Recipe.Name}";
		args.Request.Data.Properties.Description = Recipe.Details ?? "Chefs Recipe";
		args.Request.Data.SetText(await CreateShareText());
	}

	private async ValueTask<string> CreateShareText()
	{
		var shareTextBuilder = new StringBuilder();
		var steps = await Steps;

		foreach (var step in steps)
		{
			shareTextBuilder.AppendLine($"Step {step.Number}: {step.Name}")
							.AppendLine($"Ingredients: {string.Join(", ", step.Ingredients ?? ImmutableList<string>.Empty)}")
							.AppendLine($"Description: {step.Description}")
							.AppendLine();
		}

		return shareTextBuilder.ToString();
	}

#if WINDOWS
	[ComImport]
	[Guid("3A3DCD6C-3EAB-43DC-BCDE-45671CE800C8")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IDataTransferManagerInterop
	{
		IntPtr GetForWindow([In] IntPtr appWindow, [In] ref Guid riid);
		void ShowShareUIForWindow(IntPtr appWindow, ShareUIOptions options);
	}
#endif
}
