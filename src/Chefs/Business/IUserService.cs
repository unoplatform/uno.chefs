using Chefs.Settings;

namespace Chefs.Business;

public interface IUserService
{
    ValueTask<User> GetUser(CancellationToken ct);
    ValueTask<ChefApp> GetChefSettings(CancellationToken ct);
    Task SetCheffSettings(ChefApp chefSettings, CancellationToken ct);
}
