using Chefs.Data;
using Chefs.Settings;
using Uno.Extensions.Configuration;

namespace Chefs.Business;

public class UserService : IUserService
{
    private readonly IUserEndpoint _userEndpoint;
    private readonly IWritableOptions<ChefApp> _chefAppOptions;

    public UserService(IUserEndpoint userEndpoint, IWritableOptions<ChefApp> chefAppOptions)
    {
        _userEndpoint = userEndpoint;
        _chefAppOptions = chefAppOptions;
    }

    public async ValueTask<ChefApp> GetChefSettings(CancellationToken ct) => _chefAppOptions.Value;

    public async ValueTask<User> GetUser(CancellationToken ct)
    {
        var userData = await _userEndpoint.GetUser("james.bondi@gmail.com", ct);
        var user = new User(userData);
        return user;
    }

    public async Task SetCheffSettings(ChefApp chefSettings, CancellationToken ct) => await _chefAppOptions.UpdateAsync(_ => new ChefApp()
        {
            IsDark = chefSettings.IsDark,
            Notification = chefSettings.Notification,
            AccentColor = chefSettings.AccentColor,
        });
}
