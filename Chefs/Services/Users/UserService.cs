using Chefs.Services.Clients;

namespace Chefs.Services.Users;

public class UserService(
	ChefsApiClient client,
	IWritableOptions<Credentials> credentialOptions)
	: IUserService
{
	private readonly IWritableOptions<Credentials> _credentialOptions = credentialOptions;

	private IState<User> _user => State.Async(this, GetCurrent);

	public IFeed<User> User => _user;

	public async ValueTask<IImmutableList<User>> GetPopularCreators(CancellationToken ct)
	{
		var popularCreatorsData = await client.Api.User.PopularCreators.GetAsync(cancellationToken: ct);
		return popularCreatorsData?.Select(data => new User(data)).ToImmutableList() ?? ImmutableList<User>.Empty;
	}

	public async ValueTask<User> GetCurrent(CancellationToken ct)
	{
		var currentUserData = await client.Api.User.Current.GetAsync(cancellationToken: ct);
		return new User(currentUserData);
	}

	public async ValueTask<User> GetById(Guid userId, CancellationToken ct)
	{
		var userData = await client.Api.User[userId].GetAsync(cancellationToken: ct);
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
