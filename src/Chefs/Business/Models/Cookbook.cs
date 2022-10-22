using System.Collections.Immutable;
using Chefs.Data;

namespace Chefs.Business;

public record Cookbook
{
    internal Cookbook(CookbookData cookbookData)
    {
        Id = cookbookData.Id;
        UserId = cookbookData.UserId;
        Name = cookbookData.Name;
        PinsNumber = cookbookData.PinsNumber;
        Recipes = cookbookData.Recipes?
            .Select(c => new Recipe(c))
            .ToImmutableList();
    }

    public Guid Id { get; init; }
    public int? UserId { get; init; }
    public string? Name { get; init; }
    public int PinsNumber { get; init; }
    public IImmutableList<Recipe>? Recipes { get; init; }

    internal CookbookData ToData() => new()
    {
        Id = Id,
        UserId = UserId,
        Name = Name,
        PinsNumber = PinsNumber,
        Recipes = Recipes?
            .Select(c => c.ToData())
            .ToImmutableList()
    };
}
