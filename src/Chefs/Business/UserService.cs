using Chefs.Settings;

namespace Chefs.Business;

public class UserService : IUserService
{
    public ValueTask<ChefSettings> GetChefSettings(CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public ValueTask<User> GetUser(CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task SetCheffSettings(ChefSettings chefSettings, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}
