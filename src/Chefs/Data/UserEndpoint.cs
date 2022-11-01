using Chefs.Data.Models;
using System.Collections.Immutable;
using Uno.Extensions.Serialization;
using Uno.Extensions.Specialized;
using Uno.Extensions.Storage;

namespace Chefs.Data;
public class UserEndpoint : IUserEndpoint
{
    private readonly IStorage _dataService;
    private readonly ISerializer _serializer;

    private Guid? _userId;
    private List<UserData>? _users;

    public UserEndpoint(IStorage dataService, 
        ISerializer serializer) => 
        (_dataService,  _serializer) = 
        (dataService, serializer);

    public async ValueTask<bool> Authenticate(string email, string password, CancellationToken ct)
    {
       var user = (await Load())?
            .Where(u => u.Email == email && u.Password == password).FirstOrDefault();

        if (user is null)
        {
            return false;
        }

        _userId = user.Id;
        return true;
    }

    public async ValueTask<IImmutableList<UserData>> GetPopularCreators(CancellationToken ct) =>
        (await Load())?
        .Where(x=> x.Id != _userId)
        .ToImmutableList()
        ?? ImmutableList<UserData>.Empty;

    public async ValueTask<UserData> GetCurrent(CancellationToken ct)
    {
        var user = (await Load())?.Where(u => u.Id == _userId).FirstOrDefault();

        if(user is not null)
        {
            return user;
        }

        throw new Exception();
    }

    public async ValueTask Update(UserData user, CancellationToken ct)
    {
        var users = await Load();
        var oldUser = users?.Where(u => u.Id == _userId)?.FirstOrDefault();

        if(oldUser is not null)
        {
            oldUser = new UserData()
            {
                Id = user.Id,
                UrlProfileImage = user.UrlProfileImage,
                FullName = user.FullName,
                Description = user.Description,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Password = user.Password,
                Followers = user.Followers,
                Following = user.Following,
            };
            _users = users!;
        }

        throw new Exception();
    }

    //Implementation to update users in memory 
    private async ValueTask<List<UserData>> Load()
    {
        if(_users == null)
        {
            _users = (await _dataService.ReadFileAsync<List<UserData>>(_serializer, Constants.UserDataFile))!;
        }
        return _users!;
    }
}
