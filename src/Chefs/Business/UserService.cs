﻿using Chefs.Data;
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

    public async ValueTask<ChefApp> GetSettings(CancellationToken ct) => _chefAppOptions.Value;

    public async ValueTask<IImmutableList<User>> GetPopularCreators(CancellationToken ct) => (await _userEndpoint.GetPopularCreators(ct)).Select(data => new User(data)).ToImmutableList();

    public async ValueTask<User> GetCurrent(CancellationToken ct) => new User(await _userEndpoint.GetCurrent(ct));

    public async Task SetSettings(ChefApp chefSettings, CancellationToken ct) => await _chefAppOptions.UpdateAsync(_ => new ChefApp()
        {
            IsDark = chefSettings.IsDark,
            Notification = chefSettings.Notification,
            AccentColor = chefSettings.AccentColor,
        });

    public async ValueTask Update(User user, CancellationToken ct)
    {
        await _userEndpoint.Update(user.ToData(), ct);
    }
}
