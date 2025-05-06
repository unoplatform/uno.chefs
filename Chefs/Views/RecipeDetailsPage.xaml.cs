using LiveChartsCore;

namespace Chefs.Views;

public sealed partial class RecipeDetailsPage : Page
{
	public RecipeDetailsPage()
	{
		this.InitializeComponent();

		LiveCharts.Configure(config =>
			config
				.HasMap<NutritionChartItem>((nutritionChartItem, point) =>
				{
					// here we use the index as X, and the nutrition value as Y 
					return new(point, nutritionChartItem.Value);
				})
		);
	}
}
