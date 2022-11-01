using System.Collections.Immutable;

namespace Chefs.Data;

public interface IUserEndpoint
{
    ValueTask<UserData> GetCurrent(CancellationToken ct);

    ValueTask<IImmutableList<UserData>> GetPopularCreators(CancellationToken ct);

    ValueTask Update(UserData user, CancellationToken ct);

    ValueTask<bool> Authenticate(string email, string password, CancellationToken ct);
}
