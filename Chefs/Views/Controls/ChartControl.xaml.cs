using LiveChartsCore;
using LiveChartsCore.ConditionalDraw;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using Microsoft.UI;
using SkiaSharp;

namespace Chefs.Views.Controls;
public sealed partial class ChartControl : UserControl
{
	private Recipe? _recipe;


	public SolidColorBrush CarbBrush
	{
		get { return (SolidColorBrush)GetValue(CarbBrushProperty); }
		set { SetValue(CarbBrushProperty, value); }
	}

	public static readonly DependencyProperty CarbBrushProperty =
		DependencyProperty.Register("CarbBrush", typeof(SolidColorBrush), typeof(ChartControl), new PropertyMetadata(new SolidColorBrush(Colors.Black), OnBrushChanged));

	public SolidColorBrush ProteinBrush
	{
		get { return (SolidColorBrush)GetValue(ProteinBrushProperty); }
		set { SetValue(ProteinBrushProperty, value); }
	}

	public static readonly DependencyProperty ProteinBrushProperty =
		DependencyProperty.Register("ProteinBrush", typeof(SolidColorBrush), typeof(ChartControl), new PropertyMetadata(new SolidColorBrush(Colors.Black), OnBrushChanged));

	public SolidColorBrush FatBrush
	{
		get { return (SolidColorBrush)GetValue(FatBrushProperty); }
		set { SetValue(FatBrushProperty, value); }
	}

	public static readonly DependencyProperty FatBrushProperty =
		DependencyProperty.Register("FatBrush", typeof(SolidColorBrush), typeof(ChartControl), new PropertyMetadata(new SolidColorBrush(Colors.Black), OnBrushChanged));



	public SolidColorBrush DataLabelBrush
	{
		get { return (SolidColorBrush)GetValue(DataLabelBrushProperty); }
		set { SetValue(DataLabelBrushProperty, value); }
	}

	public static readonly DependencyProperty DataLabelBrushProperty =
		DependencyProperty.Register("DataLabelBrush", typeof(SolidColorBrush), typeof(ChartControl), new PropertyMetadata(new SolidColorBrush(Colors.Black), OnBrushChanged));



	public SolidColorBrush TrackBackgroundBrush
	{
		get { return (SolidColorBrush)GetValue(TrackBackgroundBrushProperty); }
		set { SetValue(TrackBackgroundBrushProperty, value); }
	}

	public static readonly DependencyProperty TrackBackgroundBrushProperty =
		DependencyProperty.Register("TrackBackgroundBrush", typeof(SolidColorBrush), typeof(ChartControl), new PropertyMetadata(new SolidColorBrush(Colors.Black), OnBrushChanged));

	private static void OnBrushChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
	{
		if (dependencyObject is not ChartControl chartControl) return;

		if (chartControl._recipe != null)
		{
			chartControl.BuildColumnChart();
			chartControl.BuildDoughnutChart();
		}
	}

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
			DataLabelsPaint = new SolidColorPaint(GetSKColorFromResource(DataLabelBrush)),
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
			Fill = new SolidColorPaint(GetSKColorFromResource(TrackBackgroundBrush))
		};
		//End

		cartesianChart.Series = new[] { rowSeriesLimit, rowSeries };
		cartesianChart.XAxes = new[] { new Axis { IsVisible = false, MaxLimit = 1000 } };
		cartesianChart.YAxes = new[] { new Axis { IsVisible = false } };
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

		pieChart.Series = c;
	}

	private SolidColorPaint GetNutritionColorPaint(string name)
	{
		return name switch
		{
			nameof(Nutrition.Carbs) => new SolidColorPaint(GetSKColorFromResource(CarbBrush)),
			nameof(Nutrition.Protein) => new SolidColorPaint(GetSKColorFromResource(ProteinBrush)),
			nameof(Nutrition.Fat) => new SolidColorPaint(GetSKColorFromResource(FatBrush)),
			_ => new SolidColorPaint(SKColors.Yellow),
		};
	}

	private SKColor GetSKColorFromResource(SolidColorBrush brush)
	{
		var color = brush.Color;
		return new SKColor(color.R, color.G, color.B, color.A);
	}
}
