using LiveChartsCore;
using LiveChartsCore.ConditionalDraw;
using LiveChartsCore.Defaults;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Extensions;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using Windows.UI;

namespace Chefs.Presentation;

public partial class RecipeDetailsModel
{
	private readonly INavigator _navigator;
	private readonly IRecipeService _recipeService;
	private readonly IUserService _userService;
	private readonly IMessenger _messenger;
	private readonly IDispatcher _dispatcher;

#if WINDOWS
	private static Color NutritionTrackBackgroundColor;
	private static Color NutritionProteinValColor;
	private static Color NutritionCarbsValColor;
	private static Color NutritionFatValColor;
	private static Color OnSurfaceColor;
	private static bool ColorsInitialized;
#else
	private static readonly Color NutritionTrackBackgroundColor = (Color)App.Current.Resources["NutritionTrackBackgroundColor"];
	private static readonly Color NutritionProteinValColor = (Color)App.Current.Resources["NutritionProteinValColor"];
	private static readonly Color NutritionCarbsValColor = (Color)App.Current.Resources["NutritionCarbsValColor"];
	private static readonly Color NutritionFatValColor = (Color)App.Current.Resources["NutritionFatValColor"];
	private static readonly Color OnSurfaceColor = (Color)App.Current.Resources["OnSurfaceColor"];
#endif

	public RecipeDetailsModel(Recipe recipe, INavigator navigator, IRecipeService recipeService, IUserService userService, IMessenger messenger, IDispatcher dispatcher)
	{
		_navigator = navigator;
		_recipeService = recipeService;
		_userService = userService;
		_dispatcher = dispatcher;

		Recipe = recipe;
		_messenger = messenger;
		messenger.Observe(Reviews, x => x.Id);

		_ = BuildCharts();
	}

	public Recipe Recipe { get; }
	public IState<User> User => State.Async(this, async ct => await _userService.GetById(Recipe.UserId, ct));
	public IState<bool> IngredientsCheck => State.Value(this, () => false);
	public IFeed<User> CurrentUser => Feed.Async(async ct => await _userService.GetCurrent(ct));
	public IListFeed<Ingredient> Ingredients => ListFeed.Async(async ct => await _recipeService.GetIngredients(Recipe.Id, ct));
	public IListState<Review> Reviews => ListState.Async(this, async ct => await _recipeService.GetReviews(Recipe.Id, ct));
	public IListFeed<Step> Steps => ListFeed.Async(async ct => await _recipeService.GetSteps(Recipe.Id, ct));

	public async ValueTask Like(Review review, CancellationToken ct)
	{
		var reviewUpdated = await _recipeService.LikeReview(review, ct);
		_messenger.Send(new EntityMessage<Review>(EntityChange.Updated, reviewUpdated));
	}

	public async ValueTask Dislike(Review review, CancellationToken ct)
	{
		var reviewUpdated = await _recipeService.DislikeReview(review, ct);
		_messenger.Send(new EntityMessage<Review>(EntityChange.Updated, reviewUpdated));
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

	public async ValueTask IngredientsChecklist(CancellationToken ct)
		=> await IngredientsCheck.Update(c => !c, ct);

	public async ValueTask Review(IImmutableList<Review> reviews, CancellationToken ct) =>
		await _navigator.NavigateRouteAsync(this, "Reviews", data: new ReviewParameter(Recipe.Id, reviews), qualifier: Qualifiers.Dialog, cancellation: ct);

	public async ValueTask Save(Recipe recipe, CancellationToken ct) =>
		await _recipeService.Save(recipe, ct);

	public async ValueTask Share(CancellationToken ct) =>
		throw new NotSupportedException("to define");

	public IEnumerable<ISeries>? DoughnutSeries { get; set; }

	public IEnumerable<ISeries>? ColumnSeries { get; set; }

	public Axis[] XAxes = { new Axis { IsVisible = false, MaxLimit = 1000 } };

	public Axis[] YAxes = { new Axis { IsVisible = false } };

	private async Task BuildColumnChart()
	{
		//Build column chart
		var _chartdata = new NutritionChartItem[]
		 {
			new(nameof(Nutrition.Fat),Recipe.Nutrition.Fat,Recipe.Nutrition.FatBase,GetNutritionColorPaint(nameof(Nutrition.Fat))),
			new(nameof(Nutrition.Carbs),Recipe.Nutrition.Carbs,Recipe.Nutrition.CarbsBase,GetNutritionColorPaint(nameof(Nutrition.Carbs))),
			new(nameof(Nutrition.Protein),Recipe.Nutrition.Protein,Recipe.Nutrition.ProteinBase, GetNutritionColorPaint(nameof(Nutrition.Protein)))
		 };

		var rowSeries = new RowSeries<NutritionChartItem>
		{
			Values = _chartdata,
			DataLabelsPaint = new SolidColorPaint(GetSKColorFromResource(OnSurfaceColor)),
			DataLabelsPosition = DataLabelsPosition.Right,
			DataLabelsFormatter = point => $"{point.Model!.Name} {point.Model!.ChartProgressVal}/{point.Model!.MaxValueRef}g",
			DataLabelsSize = 13,
			IgnoresBarPosition = true,
			MaxBarWidth = 22,
			Padding = 1,

			IsVisibleAtLegend = true
		}.OnPointMeasured(point =>
		{
			if (point.Visual is null) return;
			point.Visual.Fill = point.Model!.ColumnColor;
		});
		//End

		//Build column background
		var chartlimit = new NutritionChartItem[]
		{
			new(),
			new(),
			new()
		};

		var rowSeriesLimit = new RowSeries<NutritionChartItem>
		{
			Values = chartlimit,
			IgnoresBarPosition = true,
			MaxBarWidth = 22,
			Padding = 1,
			Fill = new SolidColorPaint(GetSKColorFromResource(NutritionTrackBackgroundColor))
		};
		//End

		ColumnSeries = new[] { rowSeriesLimit, rowSeries };
	}

	private async Task BuildDoughnutChart()
	{
		DoughnutSeries = new ISeries[]
		{
			new PieSeries<int>
			{
				Values = new []{ 5 },
				Fill = GetNutritionColorPaint(nameof(Nutrition.Fat)),
				InnerRadius = 60,
			},
			new PieSeries<int>
			{
				Values = new []{ 5},
				Fill = GetNutritionColorPaint(nameof(Nutrition.Protein)),
				InnerRadius = 60,
			},
			new PieSeries<int>
			{
				Values = new []{ 5 },
				Fill = GetNutritionColorPaint(nameof(Nutrition.Carbs)),
				InnerRadius = 60,
			}
		};
	}

	private SolidColorPaint GetNutritionColorPaint(string name)
	{
		return name switch
		{
			nameof(Nutrition.Carbs) => new SolidColorPaint(GetSKColorFromResource(NutritionCarbsValColor)),
			nameof(Nutrition.Protein) => new SolidColorPaint(GetSKColorFromResource(NutritionProteinValColor)),
			nameof(Nutrition.Fat) => new SolidColorPaint(GetSKColorFromResource(NutritionFatValColor)),
			_ => new SolidColorPaint(SKColors.Red),
		};
	}

	private SKColor GetSKColorFromResource(Color resourceColor)
	{
		return new SKColor(resourceColor.R, resourceColor.G, resourceColor.B, resourceColor.A);
	}

	private async Task BuildCharts()
	{
#if WINDOWS
		await _dispatcher.ExecuteAsync(async ct =>
		{
			await EnsureResourceColors();
#endif
		await BuildDoughnutChart();
		await BuildColumnChart();
#if WINDOWS
		}, CancellationToken.None);
#endif
	}

#if WINDOWS
	private async Task EnsureResourceColors()
	{
		if (!ColorsInitialized)
		{
			await _dispatcher.ExecuteAsync(ct =>
			{
				NutritionTrackBackgroundColor = (Color)App.Current.Resources["NutritionTrackBackgroundColor"];
				NutritionProteinValColor = (Color)App.Current.Resources["NutritionProteinValColor"];
				NutritionCarbsValColor = (Color)App.Current.Resources["NutritionCarbsValColor"];
				NutritionFatValColor = (Color)App.Current.Resources["NutritionFatValColor"];
				OnSurfaceColor = (Color)App.Current.Resources["OnSurfaceColor"];

				ColorsInitialized = true;
			}, CancellationToken.None);
		}
	}
#endif
}
