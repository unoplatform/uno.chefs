using Chefs.Data;
using System.Collections.Immutable;

namespace Chefs.Business;
public class CookbookService : ICookbookService
{
    private readonly ICookbookEndpoint _cookbookEndpoint;

    public CookbookService(ICookbookEndpoint cookEndpoint)
        => _cookbookEndpoint = cookEndpoint;

    public async ValueTask<Cookbook> Create(string name, IImmutableList<Recipe> recipes, CancellationToken ct)
    {
        var cookbookData = new CookbookData
        {
            Id = Guid.NewGuid(),
            Name = name,
            Recipes = recipes?
            .Select(i => i.ToData())
            .ToList()
        };

        await _cookbookEndpoint
        .Create(cookbookData, ct);

        return new Cookbook(cookbookData);
    }

    public async ValueTask<Cookbook> Update(Cookbook cookbook, IImmutableList<Recipe> recipes, CancellationToken ct) 
		=> new(await _cookbookEndpoint.Update(cookbook.ToData(recipes), ct));

    public async ValueTask Update(Cookbook cookbook, CancellationToken ct) 
		=> await _cookbookEndpoint.Update(cookbook.ToData(), ct);

    public async ValueTask Save(Cookbook cookbook, CancellationToken ct) 
		=> await _cookbookEndpoint.Save(cookbook.ToData(), ct);

    public async ValueTask<IImmutableList<Cookbook>> GetSaved(CancellationToken ct) 
		=> (await _cookbookEndpoint.GetSaved(ct))
	        .Select(c => new Cookbook(c))
	        .ToImmutableList();

    public async ValueTask<IImmutableList<Cookbook>> GetByUser(Guid userId, CancellationToken ct) 
		=> (await _cookbookEndpoint.GetAll(ct))
	        .Where(r => r.UserId == userId)
	        .Select(x => new Cookbook(x))
	        .ToImmutableList() ?? ImmutableList<Cookbook>.Empty;
}
