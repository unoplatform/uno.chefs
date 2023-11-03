
namespace Chefs.Business.Models;

public partial record Nutrition
{
	internal Nutrition(NutritionData? nutritionData)
	{
		Protein = nutritionData?.Protein;
		Carbs = nutritionData?.Carbs;
		Fat = nutritionData?.Fat;
		ProteinBase = nutritionData?.ProteinBase;
		CarbsBase = nutritionData?.CarbsBase;
		FatBase = nutritionData?.FatBase;
	}

	public double? Protein { get; }
	public double? ProteinBase { get; }
	public double? Carbs { get; }
	public double? CarbsBase { get; }
	public double? Fat { get; }
	public double? FatBase { get; }
}
