using System.Collections.Immutable;
using System.Xml.Linq;
using Chefs.Data;

namespace Chefs.Business;

public record Step
{
    public Step(StepData stepData)
    {
        Number = stepData.Number;
        CookTime = stepData.CookTime;
        Cookware = stepData.Cookware;
        Ingredients = stepData.Ingredients?
            .Select(i => new Ingredient(i))
            .ToImmutableList();
        Description = stepData.Description;
    }

    public int Number { get; init; }
    public TimeSpan CookTime { get; init; }
    public IImmutableList<string>? Cookware { get; init; }
    public IImmutableList<Ingredient>? Ingredients { get; init; }
    public string? Description { get; init; }

    internal StepData ToData() => new()
    {
        Number = Number,
        CookTime = CookTime,
        Cookware = Cookware,
        Ingredients = Ingredients?
            .Select(i => i.ToData())
            .ToImmutableList(),
        Description = Description
    };
}
