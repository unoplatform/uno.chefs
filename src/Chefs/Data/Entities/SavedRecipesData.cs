namespace Chefs.Data;

public class SavedRecipesData
{
	public Guid UserId { get; set; }
	public Guid[]? SavedRecipes { get; set; }
}
