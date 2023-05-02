namespace Chefs.Business.Models;

public record Ingredient
{
	public Ingredient(IngredientData ingredientData)
	{
		UrlIcon = ingredientData.UrlIcon;
		Name = ingredientData.Name;
		Quantity = ingredientData.Quantity;
	}
	public string? UrlIcon { get; set; }
	public string? Name { get; init; }
	public string? Quantity { get; init; }
	public string? NameQuantity => string.Concat(Name, " - ", Quantity);

	internal IngredientData ToData() => new()
	{
		UrlIcon = UrlIcon,
		Name = Name,
		Quantity = Quantity
	};
}
