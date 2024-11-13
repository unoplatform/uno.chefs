using LiveChartsCore;
using LiveChartsCore.ConditionalDraw;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using Windows.UI;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Chefs.Views.Controls;
public sealed partial class ChartControl : UserControl
{
	private Recipe? _recipe;



	public ChartControl()
	{
		this.InitializeComponent();

		_recipe = DataContext as Recipe;
		if (_recipe != null)
		{
			BuildColumnChart();
			BuildDoughnutChart();
		}

		DataContextChanged += OnDataContextChanged;
	}

	private void OnDataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
	{
		_recipe = args.NewValue as Recipe;

		if (_recipe != null)
		{
			BuildColumnChart();
			BuildDoughnutChart();
		}
	}

	private void BuildColumnChart()
	{
		//Build column chart
		var _chartdata = new NutritionChartItem[]
		 {
			new(nameof(Nutrition.Fat),_recipe?.Nutrition.Fat,_recipe?.Nutrition.FatBase,GetNutritionColorPaint(nameof(Nutrition.Fat))),
			new(nameof(Nutrition.Carbs),_recipe?.Nutrition.Carbs,_recipe?.Nutrition.CarbsBase,GetNutritionColorPaint(nameof(Nutrition.Carbs))),
			new(nameof(Nutrition.Protein),_recipe?.Nutrition.Protein,_recipe?.Nutrition.ProteinBase, GetNutritionColorPaint(nameof(Nutrition.Protein)))
		 };

		var rowSeries = new RowSeries<NutritionChartItem>
		{
			Values = _chartdata,
			DataLabelsPaint = new SolidColorPaint(GetSKColorFromResource("NutritionDataLabelColor")),
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
			Fill = new SolidColorPaint(GetSKColorFromResource("NutritionTrackBackgroundColor"))
		};
		//End

		if (_recipe is not null)
		{
			_recipe.Series = new[] { rowSeriesLimit, rowSeries };
			_recipe.XAxes = new[] { new Axis { IsVisible = false, MaxLimit = 1000 } };
			_recipe.YAxes = new[] { new Axis { IsVisible = false } };
		}
	}

	private void BuildDoughnutChart()
	{
		var c = new ISeries[]
		{
			new PieSeries<int>
			{
				Values = new []{ 5 },
				Fill = GetNutritionColorPaint(nameof(Nutrition.Fat)),
				InnerRadius = 60,
			},
			new PieSeries<int>
			{
				Values = new []{ 5 },
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

		//pieChart.Series = c;
	}

	private SolidColorPaint GetNutritionColorPaint(string name)
	{
		return name switch
		{
			nameof(Nutrition.Carbs) => new SolidColorPaint(GetSKColorFromResource("NutritionCarbsValColor")),
			nameof(Nutrition.Protein) => new SolidColorPaint(GetSKColorFromResource("NutritionProteinValColor")),
			nameof(Nutrition.Fat) => new SolidColorPaint(GetSKColorFromResource("NutritionFatValColor")),
			_ => new SolidColorPaint(SKColors.Yellow),
		};
	}

	private SKColor GetSKColorFromResource(string resourceKey)
	{
		if (Resources.TryGetValue(resourceKey, out var resource) && resource is Color color)
		{
			return new SKColor(color.R, color.G, color.B, color.A);
		}

		return SKColor.Empty;
	}
}
