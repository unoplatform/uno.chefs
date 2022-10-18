using System.Collections.Immutable;

namespace Chefs.Data;

public interface IUserEndpoint
{
    ValueTask<UserData> GetUserInformation(CancellationToken ct);
}
