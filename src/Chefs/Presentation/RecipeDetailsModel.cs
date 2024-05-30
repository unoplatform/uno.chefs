using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;

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

	public async ValueTask Like(Review review, CancellationToken ct) =>
		await _recipeService.LikeReview(review, ct);

	public async ValueTask Dislike(Review review, CancellationToken ct) =>
		await _recipeService.DislikeReview(review, ct);

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

	public async ValueTask Save(CancellationToken ct) => 
		await _recipeService.Save(Recipe, ct);

	/*[ComImport, Guid("3A3DCD6C-3EAB-43DC-BCDE-45671CE800C8")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	interface IDataTransferManagerInterop
	{
		DataTransferManager GetForWindow([In] IntPtr appWindow, [In] ref Guid riid);
		void ShowShareUIForWindow(IntPtr appWindow);
	}

	static readonly Guid _dtm_iid =
		new Guid(0xa5caee9b, 0x8708, 0x49d1, 0x8d, 0x36, 0x67, 0xd2, 0x5a, 0x8d, 0xa0, 0x0c);*/

	public async ValueTask Share(CancellationToken ct)
	{
#if WINDOWS
		/*var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
		var DataTransferManagerInterop = (IDataTransferManagerInterop)WindowsRuntimeMarshal.GetActivationFactory(typeof(DataTransferManager));

		var dtm = DataTransferManagerInterop.GetForWindow(hwnd, _dtm_iid);
		//dtm.DataRequested += OnDataRequested;

		//IntPtr dataTransferManagerPtr = interop.GetForWindow(hWnd, _dtm_iid);
		//var dataTransferManager = Marshal.GetObjectForIUnknown(dataTransferManagerPtr) as DataTransferManager;

		//dataTransferManager.DataRequested += (sender, args) =>
		//{
		//	args.Request.Data.Properties.Title = $"Sharing {Recipe.Name}";
		//	args.Request.Data.Properties.Description = "Description";
		//	args.Request.Data.SetText(Recipe.Details ?? "Details");
		//	args.Request.Data.RequestedOperation = DataPackageOperation.Copy;
		//};

		//interop.ShowShareUIForWindow(hWnd);*/
#else
        var dataTransferManager = DataTransferManager.GetForCurrentView();
        dataTransferManager.DataRequested += DataRequested;
        DataTransferManager.ShowShareUI();
#endif
	}

	private void DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
	{
		args.Request.Data.Properties.Title = $"Sharing {Recipe.Name}";
		args.Request.Data.Properties.Description = "Description";
		args.Request.Data.SetText(Recipe.Details ?? "Details");
	}
}
