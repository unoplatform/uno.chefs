using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using LiveChartsCore;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using SkiaSharp;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Chefs.Views;
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
		};
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

		MyCartesian.Series = new[] { rowSeriesLimit, rowSeries };
		MyCartesian.XAxes = new[] { new Axis { IsVisible = false, MaxLimit = 1000 } };
		MyCartesian.YAxes = new[] { new Axis { IsVisible = false } };
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

		MyPie.Series = c;
	}

	private SolidColorPaint GetNutritionColorPaint(string name)
	{
		return name switch
		{
			nameof(Nutrition.Carbs) => new SolidColorPaint(GetSKColorFromResource("NutritionCarbsValColor")),
			nameof(Nutrition.Protein) => new SolidColorPaint(GetSKColorFromResource("NutritionProteinValColor")),
			nameof(Nutrition.Fat) => new SolidColorPaint(GetSKColorFromResource("NutritionFatValColor")),
			_ => new SolidColorPaint(SKColors.Red),
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
