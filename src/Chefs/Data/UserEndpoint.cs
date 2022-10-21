using System.Collections.Immutable;
using Uno.Extensions.Serialization;
using Uno.Extensions.Storage;

namespace Chefs.Data;
public class UserEndpoint : IUserEndpoint
{
    public const string UserDataFile = "users.json";

    private readonly IStorage _dataService;
    private readonly ISerializer _serializer;

    private UserData? _user;

    public UserEndpoint(IStorage dataService, ISerializer serializer) 
        => (_dataService, _serializer) = (dataService, serializer);

    public async ValueTask<bool> Authenticate(string email, string password, CancellationToken ct)
    {
       var user = (await _dataService.ReadFileAsync<IImmutableList<UserData>>(_serializer, UserDataFile))?
            .Where(u => u.Email == _user?.Email && u.Password == _user?.Password).FirstOrDefault();

        if (user is null)
        {
            return false;
        }

        _user = user;
        return true;
    }

    public async ValueTask<UserData> GetUser(CancellationToken ct) => (await _dataService.ReadFileAsync<IImmutableList<UserData>>(_serializer, UserDataFile))?
            .Where(u => u.Email == _user?.Email).FirstOrDefault()
            ?? new UserData();
}
