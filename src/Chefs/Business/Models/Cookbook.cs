using System.Collections.Immutable;
using Chefs.Data;

namespace Chefs.Business;

public record Cookbook
{
    internal Cookbook(CookbookData cookbookData)
    {
        Name = cookbookData.Name;
        PinsNumber = cookbookData.PinsNumber;
        Recipes = cookbookData.Recipes?
            .Select(c => new Recipe(c))
            .ToImmutableList();
        Save = cookbookData.Save;
    }
    public string? Name { get; init; }
    public int PinsNumber { get; init; }
    public IImmutableList<Recipe>? Recipes { get; init; }
    public bool Save { get; init; }

    internal CookbookData ToData() => new()
    {
        Name = Name,
        PinsNumber = PinsNumber,
        Recipes = Recipes?
            .Select(c => c.ToData())
            .ToImmutableList(),
        Save = Save
    };
}
