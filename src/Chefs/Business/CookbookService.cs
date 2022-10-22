using Chefs.Data;
using System.Collections.Immutable;

namespace Chefs.Business;
public class CookbookService : ICookbookService
{
    private readonly ICookbookEndpoint _cookbookEndpoint;
    private readonly IUserEndpoint _userEndpoint;

    public CookbookService(ICookbookEndpoint cookEndpoint, IUserEndpoint userEndpoint)
        => (_cookbookEndpoint, _userEndpoint) = (cookEndpoint, userEndpoint);

    public async ValueTask AddUserCookbook(Cookbook cookbook, CancellationToken ct) => await _cookbookEndpoint
        .CreateCookbook(cookbook.ToData(), ct);

    public async ValueTask SaveCookbook(Cookbook cookbook, CancellationToken ct) => await _cookbookEndpoint
        .SaveCookbook(cookbook.ToData(), ct);

    public async ValueTask<IImmutableList<Cookbook>> GetSavedCookbooks(CancellationToken ct) => (await _cookbookEndpoint
        .GetSavedCookbooks(ct))
        .Select(c => new Cookbook(c))
        .ToImmutableList();

    public async ValueTask<IImmutableList<Cookbook>> GetUserCookbooks(CancellationToken ct) => (await _cookbookEndpoint
        .GetSavedCookbooks(ct))
        .Select(c => new Cookbook(c))
        .ToImmutableList();
}
