using LiveChartsCore.SkiaSharpView.Painting;

namespace Chefs.Business.Models;

public partial record NutritionChartItem
{
	public NutritionChartItem(int chartTrackVal = 100)
	{
		Value = chartTrackVal;
	}

	public NutritionChartItem(string? name, double? value, double? maxValueRef, SolidColorPaint? columnColor = default)
	{
		Name = name;
		ColumnColor = columnColor;
		ChartProgressVal = value;

		var _val = value ?? 0;
		var _maxValueRef = maxValueRef ?? 0;
		var _tempValue = (_val / _maxValueRef) * 100;

		Value = _tempValue;
		MaxValueRef = _maxValueRef;
	}

	public string? Name { get; }

	public double? ChartProgressVal { get; }

	public double Value { get; }

	public double MaxValueRef { get; }

	public SolidColorPaint? ColumnColor { get; }
}