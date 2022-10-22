using System.Collections.Immutable;

namespace Chefs.Data;
public interface ICookbookEndpoint
{
    ValueTask CreateCookbook(CookbookData cookbook, CancellationToken ct);

    ValueTask SaveCookbook(CookbookData cookbook, CancellationToken ct);

    ValueTask<IImmutableList<CookbookData>> GetSavedCookbooks(CancellationToken ct);
}
