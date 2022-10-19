using Chefs.Data;
using Chefs.Settings;
using Uno.Extensions.Configuration;

namespace Chefs.Business;

public class UserService : IUserService
{
    private readonly IUserEndpoint _userEndpoint;
    private readonly IWritableOptions<ChefApp> _chefAppOptions;
    private readonly IWritableOptions<AuthenticationOptions> _authenticationOptions;
    private User? _user;

    public UserService(IUserEndpoint userEndpoint, 
        IWritableOptions<ChefApp> chefAppOptions, 
        IWritableOptions<AuthenticationOptions> authenticationOptions)
    {
        _userEndpoint = userEndpoint;
        _chefAppOptions = chefAppOptions;
        _authenticationOptions = authenticationOptions;
    }

    public async ValueTask<User?> Auth(string email, CancellationToken ct)
    {
        var user = await _userEndpoint.GetUser(email, ct);
        if (user != null)
        {
            _user = new User(user);
            await _authenticationOptions.UpdateAsync(_ => new AuthenticationOptions()
            {
                UserName = email,
                SaveCredentials = true
            });

            return _user;
        }
        return null;
    }

    public async ValueTask<ChefApp> GetChefSettings(CancellationToken ct) => _chefAppOptions.Value;

    public async ValueTask<User> GetUser(CancellationToken ct)
    {
        var userData = await _userEndpoint.GetUser(_user!.Email!, ct);
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
