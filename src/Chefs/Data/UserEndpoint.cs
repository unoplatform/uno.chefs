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

    private Guid? _userId = new Guid("3c896419-e280-40e7-8552-240635566fed");
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
        int userIndex = (await Load())?.FindIndex(u => u.Id == _userId) ?? 0;

        if (userIndex is not -1)
        {
            if (!(_users![userIndex].IsCurrent)) _users![userIndex].IsCurrent = true;
            return _users![userIndex];
        }

        throw new Exception();
    }

    public async ValueTask Update(UserData user, CancellationToken ct)
    {
        var users = await Load();
        int userIndex = users?.FindIndex(u => u.Id == _userId) ?? 0;

        if (userIndex is not -1)
        {
            users![userIndex] = new UserData()
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
                Recipes = user.Recipes
            };
            _users = users!;
        }
        else
        {
            throw new Exception();
        }
    }

    public async ValueTask<UserData> GetById(Guid userId, CancellationToken ct) => (await Load())?.Where(u => u.Id == userId).FirstOrDefault() ?? throw new Exception();

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
