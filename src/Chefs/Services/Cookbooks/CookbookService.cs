namespace Chefs.Services.Cookbooks;

public class CookbookService : ICookbookService
{
	private readonly ICookbookEndpoint _cookbookEndpoint;
	private readonly IMessenger _messenger;

	public CookbookService(ICookbookEndpoint cookEndpoint, IMessenger messenger)
	{
		_cookbookEndpoint = cookEndpoint;
		_messenger = messenger;
	}

	public IListFeed<Cookbook> SavedCookbooks => ListFeed.Async(GetSaved);

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

		await _cookbookEndpoint.Create(cookbookData, ct);

		return new Cookbook(cookbookData);
	}

	public async ValueTask<Cookbook> Update(Cookbook cookbook, IImmutableList<Recipe> recipes, CancellationToken ct)
	{
		Cookbook newCookbook = new(await _cookbookEndpoint.Update(cookbook.ToData(recipes), ct));

		_messenger.Send(newCookbook);
		return newCookbook;
	}

	public async ValueTask Update(Cookbook cookbook, CancellationToken ct)
	{
		await _cookbookEndpoint.Update(cookbook.ToData(), ct);
		_messenger.Send(cookbook);
	}

	public async ValueTask Save(Cookbook cookbook, CancellationToken ct)
	{
		await _cookbookEndpoint.Save(cookbook.ToData(), ct);
		_messenger.Send(cookbook);
	}

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
