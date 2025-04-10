
namespace Chefs.Data;

public class NutritionData
{
	internal NutritionData(double protein, double carbs, double fat, double proteinBase, double carbsBase, double fatBase)
	{
		Protein = protein;
		Carbs = carbs;
		Fat = fat;
		ProteinBase = proteinBase;
		CarbsBase = carbsBase;
		FatBase = fatBase;
	}

	public double Protein { get; }
	public double ProteinBase { get; }
	public double Carbs { get; }
	public double CarbsBase { get; }
	public double Fat { get; }
	public double FatBase { get; }

}
