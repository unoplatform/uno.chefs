using Chefs.Services.Clients;
using Microsoft.Kiota.Abstractions.Serialization;
using UserData = Chefs.Services.Clients.Models.UserData;

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
