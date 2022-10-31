using Chefs.Data;
using System.Collections.Immutable;

namespace Chefs.Business;
public class CookbookService : ICookbookService
{
    private readonly ICookbookEndpoint _cookbookEndpoint;

    public CookbookService(ICookbookEndpoint cookEndpoint)
        => _cookbookEndpoint = cookEndpoint;

    public async ValueTask Create(Cookbook cookbook, CancellationToken ct) => await _cookbookEndpoint
        .Create(cookbook.ToData(), ct);

    public async ValueTask Save(Cookbook cookbook, CancellationToken ct) => await _cookbookEndpoint
        .Save(cookbook.ToData(), ct);

    public async ValueTask<IImmutableList<Cookbook>> GetSaved(CancellationToken ct) => (await _cookbookEndpoint
        .GetSaved(ct))
        .Select(c => new Cookbook(c))
        .ToImmutableList();

    public async ValueTask<IImmutableList<Cookbook>> GetByUser(Guid userId, CancellationToken ct) =>
        (await _cookbookEndpoint.GetAll(ct))
        .Where(r => r.UserId == userId)
        .Select(x => new Cookbook(x))
        .ToImmutableList() ?? ImmutableList<Cookbook>.Empty;
}
