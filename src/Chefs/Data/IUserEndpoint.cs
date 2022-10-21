using System.Collections.Immutable;

namespace Chefs.Data;

public interface IUserEndpoint
{
    ValueTask<UserData> GetUser(CancellationToken ct);

    ValueTask<bool> Authenticate(string email, string password, CancellationToken ct);
}
