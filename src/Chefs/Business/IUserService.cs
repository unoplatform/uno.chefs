using System.Collections.Immutable;
using Chefs.Settings;

namespace Chefs.Business;

public interface IUserService
{
    ValueTask<User> GetUser(CancellationToken ct);
    ValueTask<ChefSettings> GetChefSettings(CancellationToken ct);
    Task SetCheffSettings(ChefSettings chefSettings, CancellationToken ct);
}
