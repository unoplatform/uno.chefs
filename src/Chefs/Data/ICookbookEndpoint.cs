using System.Collections.Immutable;

namespace Chefs.Data;
public interface ICookbookEndpoint
{
    ValueTask AddUserCookbook(CookbookData cookbook, int userId, CancellationToken ct);

    ValueTask SaveCookbook(CookbookData cookbook, int userId, CancellationToken ct);

    ValueTask<IImmutableList<CookbookData>> GetSavedCookbooks(int userId, CancellationToken ct);

    ValueTask<IImmutableList<CookbookData>> GetUserCookbooks(int userId, CancellationToken ct);
}
