namespace Chefs.Services;

public class MockCookbookEndpoints(string basePath, ISerializer serializer) : BaseMockEndpoint(serializer)
{
	public string HandleCookbooksRequest(HttpRequestMessage request)
	{
		var cookbooks = LoadData<List<CookbookData>>("Cookbooks.json") ?? new List<CookbookData>();

		if (request.RequestUri.AbsolutePath == "/api/Cookbook")
		{
			return serializer.ToString(cookbooks);
		}

		//Retrieving saved cookbooks for a user
		if (request.RequestUri.AbsolutePath.Contains("/api/Cookbook/saved") && request.Method == HttpMethod.Get)
		{
			var savedCookbooksIds = LoadData<List<Guid>>("SavedCookbooks.json") ?? new List<Guid>();
			var savedCookbooks = cookbooks
				.Where(cb => savedCookbooksIds.Contains((Guid)cb.Id))
				.ToList();
			return serializer.ToString(savedCookbooks);
		}

		//Creating a new cookbook
		if (request.RequestUri.AbsolutePath == "/api/Cookbook" && request.Method == HttpMethod.Post)
		{
			var cookbook = serializer.FromString<CookbookData>(request.Content.ReadAsStringAsync().Result);
			var queryParams = request.RequestUri.Query;
			var userId = ExtractUserIdFromQuery(queryParams);
			cookbook.UserId = Guid.Parse(userId);

			cookbooks?.Add(cookbook);
			File.WriteAllText(Path.Combine(basePath, "Cookbooks.json"), serializer.ToString(cookbooks));

			return serializer.ToString(cookbook);
		}

		//Updating a cookbook
		if (request.RequestUri.AbsolutePath == "/api/Cookbook" && request.Method == HttpMethod.Put)
		{
			var updatedCookbook = serializer.FromString<CookbookData>(request.Content.ReadAsStringAsync().Result);
			var cookbookItem = cookbooks?.FirstOrDefault(c => updatedCookbook != null && c.Id == updatedCookbook.Id);

			if (cookbookItem != null)
			{
				cookbookItem.Name = updatedCookbook?.Name;
				cookbookItem.Recipes = updatedCookbook?.Recipes;
				File.WriteAllText(Path.Combine(basePath, "Cookbooks.json"), serializer.ToString(cookbooks));
				return serializer.ToString(cookbookItem);
			}

			return "NotFound";
		}

		return "{}";
	}

	private string? ExtractUserIdFromQuery(string queryParams)
	{
		var queryDictionary = System.Web.HttpUtility.ParseQueryString(queryParams);
		return queryDictionary["userId"];
	}
}
