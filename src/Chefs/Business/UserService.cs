using Chefs.Data;
using Chefs.Settings;
using System.Collections.Immutable;
using Uno.Extensions.Configuration;

namespace Chefs.Business;

public class UserService : IUserService
{
    private readonly IUserEndpoint _userEndpoint;
    private readonly IWritableOptions<AppConfig> _chefAppOptions;
    private readonly IWritableOptions<Credentials> _credentialOptions;
    private Signal _userSignal = new();

    public UserService(
        IUserEndpoint userEndpoint,
        IWritableOptions<AppConfig> chefAppOptions,
        IWritableOptions<Credentials> credentialOptions)
        => (_userEndpoint, _chefAppOptions, _credentialOptions) = (userEndpoint, chefAppOptions, credentialOptions);

    public IFeed<User> UserFeed => Feed<User>.Async(async (ct) => await GetCurrent(ct) is { } user ? user : Option.Undefined<User>(), _userSignal);

    public async ValueTask<AppConfig> GetSettings(CancellationToken ct) 
        => _chefAppOptions.Value;

    public async ValueTask<IImmutableList<User>> GetPopularCreators(CancellationToken ct)
        => (await _userEndpoint.GetPopularCreators(ct)).Select(data => new User(data)).ToImmutableList();

    public async ValueTask<User> GetCurrent(CancellationToken ct) 
        => new(await _userEndpoint.GetCurrent(ct));

    public async Task SetSettings(AppConfig chefSettings, CancellationToken ct) 
        => await _chefAppOptions.UpdateAsync(_ => new AppConfig
        {
            IsDark = chefSettings.IsDark,
            Notification = chefSettings.Notification,
            AccentColor = chefSettings.AccentColor,
        });

    public async ValueTask<User> GetById(Guid userId, CancellationToken ct) 
        => new(await _userEndpoint.GetById(userId, ct));

    public async ValueTask Update(User user, CancellationToken ct)
    {
        await _userEndpoint.Update(user.ToData(), ct);
        _userSignal.Raise();
    } 

    ///In case we need to add auth
    //public async ValueTask<bool> BasicAuthenticate(string email, string password, CancellationToken ct)
    //{
    //    var autentication = await _userEndpoint.Authenticate(email, password, ct);
    //    if (autentication)
    //    {
    //        await _credentialOptions.UpdateAsync(_ => new Credentials()
    //        {
    //            Email = email,
    //            SaveCredentials = true
    //        });

    //        return true;
    //    }

    //    return false;
    //}
}
