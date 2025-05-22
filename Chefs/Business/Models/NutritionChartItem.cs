using LiveChartsCore.SkiaSharpView.Painting;

namespace Chefs.Business.Models;

public partial record NutritionChartItem
{
	public NutritionChartItem(int chartTrackVal = 1000)
	{
		Value = chartTrackVal;
	}

	public NutritionChartItem(string? name, double? value, double? maxValueRef, SolidColorPaint? columnColor = default)
	{
		Name = name;
		ColumnColor = columnColor;
		ChartProgressVal = value;

		var val = value ?? 0;
		var maxValueRef1 = maxValueRef ?? 0;
		var tempValue = (val / maxValueRef1) * 100;

		Value = tempValue * 10;
		MaxValueRef = maxValueRef1;
	}

	public string? Name { get; }

	public double? ChartProgressVal { get; }

	public double Value { get; }

	public double MaxValueRef { get; }

	public SolidColorPaint? ColumnColor { get; }
}
