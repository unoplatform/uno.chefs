using Chefs.Data;
using System.Collections.Immutable;

namespace Chefs.Business;
public class CookbookService : ICookbookService
{
    private readonly ICookbookEndpoint _cookEndpoint;
    private readonly IUserEndpoint _userEndpoint;

    public CookbookService(ICookbookEndpoint cookEndpoint, IUserEndpoint userEndpoint)
        => (_cookEndpoint, _userEndpoint) = (cookEndpoint, userEndpoint);

    public async ValueTask AddUserCookbook(Cookbook cookbook, CancellationToken ct) => await _cookEndpoint
        .AddUserCookbook(cookbook.ToData(), (await _userEndpoint.GetUser(ct)).Id, ct);

    public async ValueTask SaveCookbook(Cookbook cookbook, CancellationToken ct) => await _cookEndpoint
        .SaveCookbook(cookbook.ToData(), (await _userEndpoint.GetUser(ct)).Id, ct);

    public async ValueTask<IImmutableList<Cookbook>> GetSavedCookbooks(CancellationToken ct) => (await _cookEndpoint
        .GetSavedCookbooks((await _userEndpoint.GetUser(ct)).Id, ct))
        .Select(c => new Cookbook(c))
        .ToImmutableList();

    public async ValueTask<IImmutableList<Cookbook>> GetUserCookbooks(CancellationToken ct) => (await _cookEndpoint
        .GetUserCookbooks((await _userEndpoint.GetUser(ct)).Id, ct))
        .Select(c => new Cookbook(c))
        .ToImmutableList();
}
