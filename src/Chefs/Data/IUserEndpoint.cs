using System.Collections.Immutable;

namespace Chefs.Data;

public interface IUserEndpoint
{
    ValueTask<UserData> GetUser(string email, CancellationToken ct);
}
