using Chefs.Business;
using Chefs.Data.Models;
using System.Collections.Immutable;
using Uno.Extensions;
using Uno.Extensions.Serialization;
using Uno.Extensions.Storage;

namespace Chefs.Data;

public class CookbookEndpoint : ICookbookEndpoint
{
    private readonly IStorage _dataService;
    private readonly ISerializer _serializer;
    private readonly IUserEndpoint _userEndpoint;

    private List<CookbookData>? _cookbooks;
    private List<SavedCookbooksData>? _savedCookbooks;

    public CookbookEndpoint(IStorage dataService, ISerializer serializer, IUserEndpoint userEndpoint)
        => (_dataService, _serializer, _userEndpoint) = (dataService, serializer, userEndpoint);

    public async ValueTask<IImmutableList<CookbookData>> GetAll(CancellationToken ct) =>
        (await LoadCookbooks()).ToImmutableList() ?? ImmutableList<CookbookData>.Empty;

    public async ValueTask Create(CookbookData cookbook, CancellationToken ct)
    {
        await LoadCookbooks();
        var user = await _userEndpoint.GetCurrent(ct);
        cookbook.UserId = user.Id;
        _cookbooks?.Add(cookbook);
    }

    public async ValueTask<CookbookData> Update(CookbookData cookbook, CancellationToken ct)
    {
        await LoadCookbooks();

        var cookbookItem = _cookbooks?.Where(c => c.Id == cookbook.Id).FirstOrDefault();
        if (cookbookItem is not null)
        {
            cookbookItem.Recipes = cookbook.Recipes;

            return cookbookItem;
        }

        throw new Exception();
    }

    public async ValueTask Save(CookbookData cookbook, CancellationToken ct)
    {
        var currentUser = await _userEndpoint.GetCurrent(ct);

        await LoadSavedCookbooks();

        var userSavedCookbooks = _savedCookbooks?.Where(x => x.UserId == currentUser.Id).FirstOrDefault();

        if (userSavedCookbooks?.SavedCookbooks is not null)
        {
            if (userSavedCookbooks.SavedCookbooks.Contains(cookbook.Id))
            {
                userSavedCookbooks.SavedCookbooks.Remove(cookbook.Id);
            }
            else
            {
                userSavedCookbooks.SavedCookbooks.Add(cookbook.Id);
            }
        }
        else
        {
            _savedCookbooks?.Add(new SavedCookbooksData { UserId = currentUser.Id, SavedCookbooks = new List<Guid> { cookbook.Id } });
        }
    }

    public async ValueTask<IImmutableList<CookbookData>> GetSaved(CancellationToken ct) 
    {
        var userId = (await _userEndpoint.GetCurrent(ct)).Id;

        return (await LoadCookbooks())
        .Where(x => x.UserId == userId)
        .ToImmutableList() ?? ImmutableList<CookbookData>.Empty;
    }

    //Implementation to update cookbooks in memory 
    private async Task<List<CookbookData>> LoadCookbooks()
    {
        if(_cookbooks == null)
        {
            _cookbooks = (await _dataService
                .ReadPackageFileAsync<List<CookbookData>>(_serializer, Constants.CookbooksDataFile));
        }
        return _cookbooks ?? new List<CookbookData>();
    }

    //Implementation to update saved cookbooks and recipes in memory 
    private async Task<List<SavedCookbooksData>> LoadSavedCookbooks()
    {
        if(_savedCookbooks == null)
        {
            _savedCookbooks = (await _dataService
                .ReadPackageFileAsync<List<SavedCookbooksData>>(_serializer, Constants.SavedCookbooksDataFile));
        }
        return _savedCookbooks ?? new List<SavedCookbooksData>();
    }
}
