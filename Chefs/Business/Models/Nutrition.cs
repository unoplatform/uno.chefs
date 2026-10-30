using NutritionData = Chefs.Client.Models.NutritionData;
namespace Chefs.Business.Models;

public partial record Nutrition
{
	internal Nutrition(NutritionData? nutritionData)
	{
		Protein = 30;
		Carbs = 101;
		Fat = 30;
		ProteinBase = 110;
		CarbsBase = 300;
		FatBase = 75;
	}

	public double? Protein { get; }
	public double? ProteinBase { get; }
	public double? Carbs { get; }
	public double? CarbsBase { get; }
	public double? Fat { get; }
	public double? FatBase { get; }
}
