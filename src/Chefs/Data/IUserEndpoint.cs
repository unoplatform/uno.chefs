using System.Collections.Immutable;

namespace Chefs.Data;

public interface IUserEndpoint
{
    ValueTask<UserData> GetUser(CancellationToken ct);

    ValueTask<IImmutableList<UserData>> GetPopularCreators(CancellationToken ct);

    ValueTask UpdateUserInfo(UserData user, CancellationToken ct);

    ValueTask<bool> Authenticate(string email, string password, CancellationToken ct);
}
