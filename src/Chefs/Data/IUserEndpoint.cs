using System.Collections.Immutable;

namespace Chefs.Data;

public interface IUserEndpoint
{
    ValueTask<IImmutableList<UserData>> GetUser(CancellationToken ct);
}
