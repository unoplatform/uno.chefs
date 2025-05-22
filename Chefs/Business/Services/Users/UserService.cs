using Chefs.Client;

namespace Chefs.Business.Services.Users;

public class UserService : IUserService
{
	public UserService(ChefsApiClient client, IWritableOptions<Credentials> credentialOptions)
	{
		_user = State.Async(this, GetCurrent);
		_client = client;
		_credentialOptions = credentialOptions;
	}

	private readonly ChefsApiClient _client;
	private readonly IWritableOptions<Credentials> _credentialOptions;

	private IState<User> _user;

	public IFeed<User> User => _user;

	public async ValueTask<IImmutableList<User>> GetPopularCreators(CancellationToken ct)
	{
		var popularCreatorsData = await _client.Api.User.PopularCreators.GetAsync(cancellationToken: ct);
		return popularCreatorsData?.Select(data => new User(data)).ToImmutableList() ?? ImmutableList<User>.Empty;
	}

	public async ValueTask<User?> GetCurrent(CancellationToken ct)
	{
		var currentUserData = await _client.Api.User.Current.GetAsync(cancellationToken: ct);
		return currentUserData is null ? default : new User(currentUserData);
	}

	public async ValueTask<User?> GetById(Guid userId, CancellationToken ct)
	{
		var userData = await _client.Api.User[userId].GetAsync(cancellationToken: ct);
		return userData is null ? default : new User(userData);
	}

	public async ValueTask Update(User user, CancellationToken ct)
	{
		await _client.Api.User.PutAsync(user.ToData(), cancellationToken: ct);
		await this._user.UpdateAsync(_ => user, ct);
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

	// return true; }

	//    return false;
	//}
}
