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
    private IImmutableList<UserData>? _users;

    public UserEndpoint(IStorage dataService, ISerializer serializer) 
        => (_dataService, _serializer) = (dataService, serializer);

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
        .RemoveAll(x=> x.Id == _userId)
        .ToImmutableList()
        ?? ImmutableList<UserData>.Empty;

    public async ValueTask<UserData> GetUser(CancellationToken ct)
    {
        var user = (await Load())?.Where(u => u.Id == _userId).FirstOrDefault();

        if(user is not null)
        {
            var savedCookbooks = (await _dataService.ReadFileAsync<List<SavedCookbooksData>>(_serializer, Constants.SavedCookbooksDataFile))?
            .Where(u => u.UserId == _userId).FirstOrDefault();

            var savedRecipes = (await _dataService.ReadFileAsync<List<SavedRecipesData>>(_serializer, Constants.SavedRecipesDataFile))?
            .Where(u => u.UserId == _userId).FirstOrDefault();

            var cookBooks = (await _dataService
            .ReadFileAsync<List<CookbookData>>(_serializer, Constants.CookbooksDataFile));

            var recipes = (await _dataService
            .ReadFileAsync<List<RecipeData>>(_serializer, Constants.RecipeDataFile));

            if (savedCookbooks?.SavedCookbooks is not null)
            {
                user.SavedCookBooks = cookBooks?.Where(x => savedCookbooks.SavedCookbooks.Any(y => y == x.Id)).ToImmutableList();
            }
            if (savedRecipes?.SavedRecipes is not null)
            {
                user.SavedRecipes = recipes?.Where(x => savedRecipes.SavedRecipes.Any(y => y == x.Id)).ToImmutableList();
            }

            return user;
        }

        throw new Exception();
    }

    public async ValueTask UpdateUserInfo(UserData user, CancellationToken ct)
    {
        var users = (await Load())?.ToList();
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

    private async ValueTask<IImmutableList<UserData>> Load()
    {
        if(_users == null)
        {
            _users = (await _dataService.ReadFileAsync<IImmutableList<UserData>>(_serializer, Constants.UserDataFile))!;
        }
        return _users!;
    }
}
