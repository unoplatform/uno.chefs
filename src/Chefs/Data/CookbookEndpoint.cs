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

    public CookbookEndpoint(IStorage dataService, ISerializer serializer, IUserEndpoint userEndpoint)
        => (_dataService, _serializer, _userEndpoint) = (dataService, serializer, userEndpoint);

    public async ValueTask CreateCookbook(CookbookData cookbook, CancellationToken ct)
    {
        var cookbooks = (await _dataService
            .ReadFileAsync<IImmutableList<CookbookData>>(_serializer, Constants.CookbooksDataFile))?
            .ToList();

        var currentUser = await _userEndpoint.GetUser(ct);

        cookbooks?.Add(new CookbookData()
        {
            Id = cookbook.Id,
            UserId = currentUser.Id,
            Name = cookbook.Name,
            PinsNumber = cookbook.PinsNumber,
            Recipes = cookbook.Recipes,
        });

        await _dataService.WriteFileAsync(Constants.CookbooksDataFile, _serializer.ToString(cookbooks!));
    }

    public async ValueTask SaveCookbook(CookbookData cookbook, CancellationToken ct)
    {
        var currentUser = await _userEndpoint.GetUser(ct);

        var savedCookbooks = await _dataService
            .ReadFileAsync<List<SavedItemsData>>(_serializer, Constants.SavedItemsDataFile);

        var userSavedCookbook = savedCookbooks?.Where(x => x.UserId == currentUser.Id).FirstOrDefault();

        if (userSavedCookbook is not null)
        {
            userSavedCookbook.SavedCookbooks = userSavedCookbook.SavedCookbooks.Concat(cookbook.Id).ToArray();
        }
        else
        {
            savedCookbooks?.Add(new SavedItemsData { UserId = currentUser.Id, SavedCookbooks = new Guid[] { cookbook.Id } });
        }

        await _dataService.WriteFileAsync(Constants.SavedItemsDataFile, _serializer.ToString(savedCookbooks!));
    }

    public async ValueTask<IImmutableList<CookbookData>> GetSavedCookbooks(CancellationToken ct)
    {
        var currentUser = await _userEndpoint.GetUser(ct);

        var cookBooks = (await _dataService
            .ReadFileAsync<List<CookbookData>>(_serializer, Constants.CookbooksDataFile));

        var savedCookbooks = (await _dataService
            .ReadFileAsync<List<SavedItemsData>>(_serializer, Constants.SavedItemsDataFile))?
            .Where(x => x.UserId == currentUser.Id).FirstOrDefault();

        if (savedCookbooks is not null && savedCookbooks.SavedCookbooks is not null)
        {
            return cookBooks?.Where(x => savedCookbooks.SavedCookbooks.Any(y => y == x.Id)).ToImmutableList() ?? ImmutableList<CookbookData>.Empty;
        }

        return ImmutableList<CookbookData>.Empty;
    }
}
