using Chefs.Settings;

namespace Chefs.Business;

public interface IUserService
{
    ValueTask<User?> Auth(string email, CancellationToken ct);
    ValueTask<User> GetUser(CancellationToken ct);
    ValueTask<ChefApp> GetChefSettings(CancellationToken ct);
    Task SetCheffSettings(ChefApp chefSettings, CancellationToken ct);
}
