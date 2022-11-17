namespace Chefs.Business;

public record Step
{
    public Step(StepData stepData)
    {
        Number = stepData.Number;
        CookTime = stepData.CookTime;
        Cookware = stepData.Cookware;
        Ingredients = stepData.Ingredients;
        Description = stepData.Description;
    }

    public int Number { get; init; }
    public TimeSpan CookTime { get; init; }
    public IImmutableList<string>? Cookware { get; init; }
    public IImmutableList<string>? Ingredients { get; init; }
    public string? Description { get; init; }

    internal StepData ToData() => new()
    {
        Number = Number,
        CookTime = CookTime,
        Cookware = Cookware,
        Ingredients = Ingredients,
        Description = Description
    };
}
