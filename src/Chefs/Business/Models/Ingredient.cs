using Chefs.Data;
using System.Net;

namespace Chefs.Business;

public record Ingredient
{
    public Ingredient(IngredientData ingredientData)
    {
        Name = ingredientData.Name;
        Quantity = ingredientData.Quantity;
    }
    //Todo: Icon?
    public string? Name { get; init; }
    public string? Quantity { get; init; }
    public string? NameQuantity => string.Concat(Name, " - ", Quantity);

    internal IngredientData ToData() => new()
    {
        Name = Name,
        Quantity = Quantity
    };
}
