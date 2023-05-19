namespace Chefs.Data;

public class CookbookData
{
	public Guid Id { get; set; }
	public Guid UserId { get; set; }
	public string? Name { get; set; }
	public List<RecipeData>? Recipes { get; set; }
}
