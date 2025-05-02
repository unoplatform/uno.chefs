using Chefs.Services.Clients;
using Microsoft.Kiota.Abstractions.Serialization;
using CookbookData = Chefs.Services.Clients.Models.CookbookData;

namespace Chefs.Services.Cookbooks;

public class CookbookService(ChefsApiClient client, IMessenger messenger, IUserService userService)
	: ICookbookService
{
	public async ValueTask<Cookbook> Create(string name, IImmutableList<Recipe> recipes, CancellationToken ct)
	{
		var currentUser = await userService.GetCurrent(ct);
		var cookbookData = new CookbookData
		{
			Id = Guid.NewGuid(),
			Name = name,
			UserId = currentUser.Id,
			Recipes = recipes?.Select(i => i.ToData()).ToList()
		};

		await client.Api.Cookbook.PostAsync(cookbookData, cancellationToken: ct);

		return new Cookbook(cookbookData);
	}

	public async ValueTask<Cookbook> Update(Cookbook cookbook, IImmutableList<Recipe> recipes, CancellationToken ct)
	{
		var updatedCookbookData = cookbook.ToData();
		updatedCookbookData.Recipes = recipes.Select(r => r.ToData()).ToList();

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
		await using var savedStream = await client.Api.Cookbook.Saved.GetAsync(config => config.QueryParameters.UserId = currentUser.Id, cancellationToken: ct);
		var jsonResponse = await new StreamReader(savedStream).ReadToEndAsync(ct);
		var savedCookbooksData = await KiotaJsonSerializer.DeserializeCollectionAsync<CookbookData>(jsonResponse, cancellationToken: ct);

		return savedCookbooksData?.Select(c => new Cookbook(c)).ToImmutableList() ?? ImmutableList<Cookbook>.Empty;
	}

	public async ValueTask<IImmutableList<Cookbook>> GetByUser(Guid userId, CancellationToken ct)
	{
		await using var allCookbooksStream = await client.Api.Cookbook.GetAsync(cancellationToken: ct);
		var jsonResponse = await new StreamReader(allCookbooksStream).ReadToEndAsync(ct);
		var allCookbooksData = await KiotaJsonSerializer.DeserializeCollectionAsync<CookbookData>(jsonResponse, cancellationToken: ct);

		return allCookbooksData?.Where(r => r.UserId == userId).Select(x => new Cookbook(x)).ToImmutableList() ?? ImmutableList<Cookbook>.Empty;
	}
}
