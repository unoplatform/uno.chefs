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

    public async ValueTask<IImmutableList<UserData>> GetUser(CancellationToken ct)
    {
        var users = await _dataService.ReadFileAsync<IImmutableList<UserData>>(_serializer, UserDataFile);

        return users ?? ImmutableList<UserData>.Empty;
    }
}
