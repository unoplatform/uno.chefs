using System.Collections.Immutable;

namespace Chefs.Data;
public interface ICookbookEndpoint
{
    ValueTask<IImmutableList<CookbookData>> GetAll(CancellationToken ct);

    ValueTask Create(CookbookData cookbook, CancellationToken ct);

    ValueTask<CookbookData> Update(CookbookData cookbook, CancellationToken ct);

    ValueTask Save(CookbookData cookbook, CancellationToken ct);

    ValueTask<IImmutableList<CookbookData>> GetSaved(CancellationToken ct);
}
