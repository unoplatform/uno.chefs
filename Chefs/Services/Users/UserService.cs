using Chefs.Services.Clients;
using Microsoft.Kiota.Abstractions.Serialization;
using UserData = Chefs.Services.Clients.Models.UserData;

namespace Chefs.Services.Users;

public class UserService(ChefsApiClient client)
	: IUserService
{
	//private readonly IWritableOptions<Credentials> _credentialOptions = credentialOptions;
	
	private IState<User> _user => State.Async(this, GetCurrent);

	public IFeed<User> User => _user;

	public async ValueTask<AppConfig> GetSettings(CancellationToken ct)
		=> new();

	public async ValueTask<IImmutableList<User>> GetPopularCreators(CancellationToken ct)
	{
		await using var responseStream = await client.Api.User.PopularCreators.GetAsync(cancellationToken: ct);
		var jsonResponse = await new StreamReader(responseStream).ReadToEndAsync(ct);
		var popularCreatorsData = await KiotaJsonSerializer.DeserializeCollectionAsync<UserData>(jsonResponse, cancellationToken: ct);
		return popularCreatorsData?.Select(data => new User(data)).ToImmutableList() ?? ImmutableList<User>.Empty;
	}

	public async ValueTask<User> GetCurrent(CancellationToken ct)
	{
		await using var responseStream = await client.Api.User.Current.GetAsync(cancellationToken: ct);
		var jsonResponse = await new StreamReader(responseStream).ReadToEndAsync(ct);
		var currentUserData = await KiotaJsonSerializer.DeserializeAsync<UserData>(jsonResponse, cancellationToken: ct);
		return new User(currentUserData);
	}

	public async Task SetSettings(AppConfig chefSettings, CancellationToken ct)
	{
		var settings = new AppConfig
		{
			Title = chefSettings.Title,
			IsDark = chefSettings.IsDark,
			Notification = chefSettings.Notification,
			AccentColor = chefSettings.AccentColor,
		};

		//await chefAppOptions.UpdateAsync(_ => settings);
	}

	public async Task UpdateSettings(CancellationToken ct, string? title = null, bool? isDark = null, bool? notification = null, string? accentColor = null)
	{
		var currentSettings = await GetSettings(ct);

		var settings = new AppConfig
		{
			Title = title ?? currentSettings.Title,
			IsDark = isDark ?? currentSettings.IsDark,
			Notification = notification ?? currentSettings.Notification,
			AccentColor = accentColor ?? currentSettings.AccentColor,
		};

		await SetSettings(settings, ct);
	}

	public async ValueTask<User> GetById(Guid userId, CancellationToken ct)
	{
		await using var responseStream = await client.Api.User[userId].GetAsync(cancellationToken: ct);
		var jsonResponse = await new StreamReader(responseStream).ReadToEndAsync(ct);
		var userData = await KiotaJsonSerializer.DeserializeAsync<UserData>(jsonResponse, cancellationToken: ct);
		return new User(userData);
	}

	public async ValueTask Update(User user, CancellationToken ct)
	{
		await client.Api.User.PutAsync(user.ToData(), cancellationToken: ct);
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
