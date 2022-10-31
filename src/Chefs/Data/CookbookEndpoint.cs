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

    private IImmutableList<CookbookData>? _cookbooks;
    private IImmutableList<SavedCookbooksData>? _savedCookbooks;

    public CookbookEndpoint(IStorage dataService, ISerializer serializer, IUserEndpoint userEndpoint)
        => (_dataService, _serializer, _userEndpoint) = (dataService, serializer, userEndpoint);

    public async ValueTask<IImmutableList<CookbookData>> GetAll(CancellationToken ct) => 
        await LoadCookbooks() ?? ImmutableList<CookbookData>.Empty;

    public async ValueTask Create(CookbookData cookbook, CancellationToken ct)
    {
        var cookbooks = (await LoadCookbooks()).ToList();

        var currentUser = await _userEndpoint.GetCurrent(ct);

        cookbooks?.Add(new CookbookData()
        {
            Id = cookbook.Id,
            UserId = currentUser.Id,
            Name = cookbook.Name,
            PinsNumber = cookbook.PinsNumber,
            Recipes = cookbook.Recipes,
        });

        _cookbooks = cookbooks!.ToImmutableList();
    }

    public async ValueTask Save(CookbookData cookbook, CancellationToken ct)
    {
        var currentUser = await _userEndpoint.GetCurrent(ct);

        var savedCookbooks = (await LoadSavedCookbooks()).ToList();

        var userSavedCookbook = savedCookbooks?.Where(x => x.UserId == currentUser.Id).FirstOrDefault();

        if (userSavedCookbook is not null)
        {
            userSavedCookbook.SavedCookbooks = userSavedCookbook.SavedCookbooks.Concat(cookbook.Id).ToArray();
        }
        else
        {
            savedCookbooks?.Add(new SavedCookbooksData { UserId = currentUser.Id, SavedCookbooks = new Guid[] { cookbook.Id } });
        }

        _savedCookbooks = savedCookbooks!.ToImmutableList();
    }

    public async ValueTask<IImmutableList<CookbookData>> GetSaved(CancellationToken ct)
    {
        var currentUser = await _userEndpoint.GetCurrent(ct);

        var cookBooks = await LoadCookbooks();

        var savedCookbooks = (await LoadSavedCookbooks())?
            .Where(x => x.UserId == currentUser.Id).FirstOrDefault();

        if (savedCookbooks is not null && savedCookbooks.SavedCookbooks is not null)
        {
            return cookBooks?.Where(x => savedCookbooks.SavedCookbooks.Any(y => y == x.Id)).ToImmutableList() ?? ImmutableList<CookbookData>.Empty;
        }

        return ImmutableList<CookbookData>.Empty;
    }

    //Implementation to update cookbooks in memory 
    private async Task<IImmutableList<CookbookData>> LoadCookbooks()
    {
        if(_cookbooks == null)
        {
            _cookbooks = (await _dataService
                .ReadFileAsync<IImmutableList<CookbookData>>(_serializer, Constants.CookbooksDataFile));
        }
        return _cookbooks ?? ImmutableList<CookbookData>.Empty;
    }

    //Implementation to update saved cookbooks and recipes in memory 
    private async Task<IImmutableList<SavedCookbooksData>> LoadSavedCookbooks()
    {
        if(_savedCookbooks == null)
        {
            _savedCookbooks = (await _dataService
                .ReadFileAsync<IImmutableList<SavedCookbooksData>>(_serializer, Constants.SavedCookbooksDataFile));
        }
        return _savedCookbooks ?? ImmutableList<SavedCookbooksData>.Empty;
    }
}
