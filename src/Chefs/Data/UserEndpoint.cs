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
    private readonly IRecipeEndpoint _recipeEndpoint;
    private readonly ICookbookEndpoint _cookbookEndpoint;

    private Guid? _userId;
    private List<UserData>? _users;

    public UserEndpoint(IStorage dataService, 
        ISerializer serializer, 
        IRecipeEndpoint recipeEndpoint, 
        ICookbookEndpoint cookbookEndpoint) 
        => (_dataService, 
        _serializer, 
        _recipeEndpoint, 
        _cookbookEndpoint) = (dataService, 
        serializer, 
        recipeEndpoint, 
        cookbookEndpoint);

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
            var savedCookbooks = (await _cookbookEndpoint.GetSaved(ct))?
            .Where(u => u.UserId == _userId);

            var savedRecipes = (await _recipeEndpoint.GetSaved(ct))?
            .Where(u => u.UserId == _userId);

            var cookBooks = (await _cookbookEndpoint.GetAll(ct));

            var recipes = (await _recipeEndpoint.GetAll(ct));

            if (savedCookbooks is not null)
            {
                user.SavedCookBooks = cookBooks?.Where(x => savedCookbooks.Any(y => y == x.Id)).ToImmutableList();
            }
            if (savedRecipes is not null)
            {
                user.SavedRecipes = recipes?.Where(x => savedRecipes.Any(y => y == x.Id)).ToImmutableList();
            }

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
            _users = users?.ToImmutableList()!;
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
