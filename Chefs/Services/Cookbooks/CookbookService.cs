using Chefs.Services.Clients;

namespace Chefs.Services.Cookbooks;

public class CookbookService(ChefsApiClient client, IMessenger messenger, IUserService userService)
	: ICookbookService
{
	public async ValueTask<Cookbook> Create(string name, IImmutableList<Recipe> recipes, CancellationToken ct)
	{
		var currentUser = await userService.GetCurrent(ct);
		var cookbookData = Cookbook.CreateData(currentUser.Id, name, recipes);

		await client.Api.Cookbook.PostAsync(cookbookData, cancellationToken: ct);

		return new Cookbook(cookbookData);
	}

	public async ValueTask<Cookbook> Update(Cookbook cookbook, IImmutableList<Recipe> recipes, CancellationToken ct)
	{
		var updatedCookbookData = cookbook.ToData(recipes);
		await client.Api.Cookbook.PutAsync(updatedCookbookData, cancellationToken: ct);

		var newCookbook = new Cookbook(updatedCookbookData);
		messenger.Send(new EntityMessage<Cookbook>(EntityChange.Updated, newCookbook));
		return newCookbook;
	}

	public async ValueTask Update(Cookbook cookbook, CancellationToken ct)
	{
		var cookbookData = cookbook.ToData();

		await client.Api.Cookbook.PutAsync(cookbookData, cancellationToken: ct);
		messenger.Send(new EntityMessage<Cookbook>(EntityChange.Updated, cookbook));
	}

	public async ValueTask Save(Cookbook cookbook, CancellationToken ct)
	{
		var currentUser = await userService.GetCurrent(ct);
		var cookbookData = cookbook.ToData();

		await client.Api.Cookbook.Save.PostAsync(
			cookbookData,
			config => config.QueryParameters.UserId = currentUser.Id,
			cancellationToken: ct
		);
		messenger.Send(new EntityMessage<Cookbook>(EntityChange.Created, cookbook));
	}

	public async ValueTask<IImmutableList<Cookbook>> GetSaved(CancellationToken ct)
	{
		var currentUser = await userService.GetCurrent(ct);
		var savedCookbooksData = await client.Api.Cookbook.Saved.GetAsync(config => config.QueryParameters.UserId = currentUser.Id, cancellationToken: ct);
		return savedCookbooksData?.Select(c => new Cookbook(c)).ToImmutableList() ?? ImmutableList<Cookbook>.Empty;
	}

	public async ValueTask<IImmutableList<Cookbook>> GetByUser(Guid userId, CancellationToken ct)
	{
		var allCookbooksData = await client.Api.Cookbook.GetAsync(cancellationToken: ct);
		return allCookbooksData?.Where(r => r.UserId == userId).Select(x => new Cookbook(x)).ToImmutableList() ?? ImmutableList<Cookbook>.Empty;
	}
}
