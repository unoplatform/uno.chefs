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

    private Guid? _user;

    public UserEndpoint(IStorage dataService, ISerializer serializer) 
        => (_dataService, _serializer) = (dataService, serializer);

    public async ValueTask<bool> Authenticate(string email, string password, CancellationToken ct)
    {
       var user = (await _dataService.ReadFileAsync<IImmutableList<UserData>>(_serializer, Constants.UserDataFile))?
            .Where(u => u.Email == email && u.Password == password).FirstOrDefault();

        if (user is null)
        {
            return false;
        }

        _user = user.Id;
        return true;
    }

    public async ValueTask<IImmutableList<UserData>> GetPopularCreators(CancellationToken ct) =>
        (await _dataService
        .ReadFileAsync<IImmutableList<UserData>>(_serializer, Constants.UserDataFile))?
        .RemoveAll(x=> x.Id == _user)
        .ToImmutableList()
        ?? ImmutableList<UserData>.Empty;

    public async ValueTask<UserData> GetUser(CancellationToken ct)
    {
        var user = (await _dataService.ReadFileAsync<IImmutableList<UserData>>(_serializer, Constants.UserDataFile))?
             .Where(u => u.Id == _user).FirstOrDefault();

        if(user is not null)
        {
            var savedItems = (await _dataService.ReadFileAsync<List<SavedItemsData>>(_serializer, Constants.SavedItemsDataFile))?
            .Where(u => u.UserId == _user).FirstOrDefault();

            var cookBooks = (await _dataService
            .ReadFileAsync<List<CookbookData>>(_serializer, Constants.CookbooksDataFile));

            var recipes = (await _dataService
            .ReadFileAsync<List<RecipeData>>(_serializer, Constants.RecipeDataFile));

            if (savedItems?.SavedCookbooks is not null)
            {
                user.SavedCookBooks = cookBooks?.Where(x => savedItems.SavedCookbooks.Any(y => y == x.Id)).ToImmutableList();
            }
            if (savedItems?.SavedRecipes is not null)
            {
                user.SavedRecipes = recipes?.Where(x => savedItems.SavedRecipes.Any(y => y == x.Id)).ToImmutableList();
            }

            return user;
        }

        throw new Exception();
    }
}
