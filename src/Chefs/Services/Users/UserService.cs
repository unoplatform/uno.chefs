namespace Chefs.Services.Users;

public class UserService : IUserService
{
	private readonly IUserEndpoint _userEndpoint;
	private readonly IWritableOptions<AppConfig> _chefAppOptions;
	private readonly IWritableOptions<Credentials> _credentialOptions;


	public UserService(
		IUserEndpoint userEndpoint,
		IWritableOptions<AppConfig> chefAppOptions,
		IWritableOptions<Credentials> credentialOptions)
		=> (_userEndpoint, _chefAppOptions, _credentialOptions) = (userEndpoint, chefAppOptions, credentialOptions);

	private IState<User> _user => State.Async(this, GetCurrent);

	public IFeed<User> User => _user;

	public IState<AppConfig> Settings => State.Async(this, GetSettings);

	public async ValueTask<AppConfig> GetSettings(CancellationToken ct)
		=> _chefAppOptions.Value;

	public async ValueTask<IImmutableList<User>> GetPopularCreators(CancellationToken ct)
		=> (await _userEndpoint.GetPopularCreators(ct)).Select(data => new User(data)).ToImmutableList();

	public async ValueTask<User> GetCurrent(CancellationToken ct)
		=> new(await _userEndpoint.GetCurrent(ct));

	public async Task SetSettings(AppConfig chefSettings, CancellationToken ct)
	{
		var settings = new AppConfig
		{
			IsDark = chefSettings.IsDark,
			Notification = chefSettings.Notification,
			AccentColor = chefSettings.AccentColor,
		};

		await _chefAppOptions.UpdateAsync(_ => settings);
		await Settings.UpdateAsync(_ => settings, ct);
	}

	public async ValueTask<User> GetById(Guid userId, CancellationToken ct)
		=> new(await _userEndpoint.GetById(userId, ct));

	public async ValueTask Update(User user, CancellationToken ct)
	{
		await _userEndpoint.Update(user.ToData(), ct);
		await _user.UpdateAsync(_ => user, ct);
	}

	//In case we need to add auth
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
