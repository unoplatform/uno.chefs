using Chefs.Data;
using Chefs.Settings;
using System.Collections.Immutable;
using Uno.Extensions.Configuration;

namespace Chefs.Business;

public class UserService : IUserService
{
    private readonly IUserEndpoint _userEndpoint;
    private readonly IWritableOptions<ChefApp> _chefAppOptions;
    private readonly IWritableOptions<AuthenticationOptions> _authenticationOptions;

    public UserService(IUserEndpoint userEndpoint,
        IWritableOptions<ChefApp> chefAppOptions,
        IWritableOptions<AuthenticationOptions> authenticationOptions)
        => (_userEndpoint, _chefAppOptions, _authenticationOptions) = (userEndpoint, chefAppOptions, authenticationOptions);

    public async ValueTask<bool> BasicAuthenticate(string email, string password, CancellationToken ct)
    {
        var autentication = await _userEndpoint.Authenticate(email, password, ct);
        if (autentication)
        {
            await _authenticationOptions.UpdateAsync(_ => new AuthenticationOptions()
            {
                Email = email,
                SaveCredentials = true
            });

            return true;
        }
        return false;
    }

    public async ValueTask<ChefApp> GetChefSettings(CancellationToken ct) => _chefAppOptions.Value;

    public ValueTask<IImmutableList<User>> GetPopularCreators(CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public async ValueTask<User> GetUser(CancellationToken ct)
    {
        var userData = await _userEndpoint.GetUser(ct);
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
