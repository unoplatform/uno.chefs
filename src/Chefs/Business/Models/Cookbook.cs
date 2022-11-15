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
        Recipes = cookbookData.Recipes?
            .Select(c => new Recipe(c))
            .ToImmutableList();
    }

    internal Cookbook() { Recipes = ImmutableList<Recipe>.Empty; }

    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public string? Name { get; init; }
    public int PinsNumber => Recipes?.Count ?? 0;
    public IImmutableList<Recipe>? Recipes { get; init; }

    internal CookbookData ToData() => new()
    {
        Id = Id,
        UserId = UserId,
        Name = Name,
        Recipes = Recipes?
            .Select(c => c.ToData())
            .ToList()
    };

    internal CookbookData ToData(IImmutableList<Recipe> recipes) => new()
    {
        Id = Id,
        UserId = UserId,
        Name = Name,
        Recipes = Recipes?
            .AddRange(recipes)
            .Select(c => c.ToData())
            .ToList()
    };
}
