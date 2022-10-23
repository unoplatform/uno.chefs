using System.Collections.Immutable;

namespace Chefs.Data;
public interface ICookbookEndpoint
{
    ValueTask Create(CookbookData cookbook, CancellationToken ct);

    ValueTask Save(CookbookData cookbook, CancellationToken ct);

    ValueTask<IImmutableList<CookbookData>> GetSaved(CancellationToken ct);
}
