using System.Collections.Immutable;
using Uno.Extensions.Serialization;
using Uno.Extensions.Storage;

namespace Chefs.Data;
public class UserEndpoint : IUserEndpoint
{
    public const string UserDataFile = "users.json";

    private readonly IStorage _dataService;
    private readonly ISerializer _serializer;

    public UserEndpoint(IStorage dataService, ISerializer serializer)
    {
        _dataService = dataService;
        _serializer = serializer;
    }

    public async ValueTask<UserData> GetUser(string email, CancellationToken ct)
    {
        var user = (await _dataService.ReadFileAsync<IImmutableList<UserData>>(_serializer, UserDataFile))?
            .Where(u => u.Email == email).FirstOrDefault();

        return user ?? new UserData();
    }
}
