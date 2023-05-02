using System.Collections.Immutable;

namespace Chefs.DataContracts;

public class StepData
{
    public string? UrlVideo { get; set; }
    public string? Name { get; set; }
    public int Number { get; set; }
    public TimeSpan CookTime { get; set; }
    public IImmutableList<string>? Cookware { get; set; }
    public IImmutableList<string>? Ingredients { get; set; }
    public string? Description { get; set; }
}
