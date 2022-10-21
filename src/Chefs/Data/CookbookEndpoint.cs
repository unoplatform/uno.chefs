using System.Collections.Immutable;
using Uno.Extensions.Serialization;
using Uno.Extensions.Storage;

namespace Chefs.Data;

public class CookbookEndpoint : ICookbookEndpoint
{
    public const string SavedCookbooksDataFile = "savedcookbooks.json";
    public const string UserCookbooksDataFile = "usercookbooks.json";

    private readonly IStorage _dataService;
    private readonly ISerializer _serializer;

    public CookbookEndpoint(IStorage dataService, ISerializer serializer)
        => (_dataService, _serializer) = (dataService, serializer);

    public async ValueTask AddUserCookbook(CookbookData cookbook, int userId, CancellationToken ct)
    {
        var cookbooks = (await _dataService
            .ReadFileAsync<IImmutableList<CookbookData>>(_serializer, UserCookbooksDataFile))?
            .Where(c => c?.UserId == userId)
            .ToList();
        cookbooks?.Add(new CookbookData()
        {
            Id = cookbook.Id,
            UserId = userId,
            Name = cookbook.Name,
            PinsNumber = cookbook.PinsNumber,
            Recipes = cookbook.Recipes,
        });
        await _dataService.WriteFileAsync(UserCookbooksDataFile, _serializer.ToString(cookbooks!));
    }

    public async ValueTask SaveCookbook(CookbookData cookbook, int userId, CancellationToken ct)
    {
        var cookbooks = (await _dataService
           .ReadFileAsync<IImmutableList<CookbookData>>(_serializer, SavedCookbooksDataFile))?
           .Where(c => c?.UserId == userId)
           .ToList();
        cookbooks?.Add(new CookbookData()
        {
            Id = cookbook.Id,
            UserId = userId,
            Name = cookbook.Name,
            PinsNumber = cookbook.PinsNumber,
            Recipes = cookbook.Recipes,
        });
        await _dataService.WriteFileAsync(SavedCookbooksDataFile, _serializer.ToString(cookbooks!));
    }

    public async ValueTask<IImmutableList<CookbookData>> GetSavedCookbooks(int userId, CancellationToken ct) => (await _dataService
        .ReadFileAsync<IImmutableList<CookbookData>>(_serializer, SavedCookbooksDataFile))?
        .Where(c => c?.UserId == userId)
        .ToImmutableList() ?? ImmutableList<CookbookData>.Empty;

    public async ValueTask<IImmutableList<CookbookData>> GetUserCookbooks(int userId, CancellationToken ct) => (await _dataService
        .ReadFileAsync<IImmutableList<CookbookData>>(_serializer, UserCookbooksDataFile))?
        .Where(c => c?.UserId == userId)
        .ToImmutableList() ?? ImmutableList<CookbookData>.Empty;
}
