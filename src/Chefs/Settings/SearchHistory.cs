using System.Collections.Immutable;

namespace Chefs.Settings;

public record SearchHistory
{
    public IImmutableList<string> Searches { get; init; } = ImmutableList<string>.Empty;
}
