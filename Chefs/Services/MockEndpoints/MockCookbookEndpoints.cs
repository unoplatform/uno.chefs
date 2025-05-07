using Chefs.DataContracts;

namespace Chefs.Services;

public class MockCookbookEndpoints(string basePath, ISerializer serializer) : BaseMockEndpoint
{
	public string HandleCookbooksRequest(HttpRequestMessage request)
	{
		var cookbooksData = LoadData("Cookbooks.json");
		var cookbooks = serializer.FromString<List<CookbookData>>(cookbooksData);
		if (request.RequestUri.AbsolutePath == "/api/cookbook")
		{
			return serializer.ToString(cookbooks);
		}

		//Retrieving saved cookbooks for a user
		if (request.RequestUri.AbsolutePath.Contains("/api/cookbook/saved") && request.Method == HttpMethod.Get)
		{
			var queryParams = request.RequestUri.Query;
			var userId = ExtractUserIdFromQuery(queryParams);
			var savedCookbooksData = LoadData("SavedCookbooks.json");
			var savedCookbooks = serializer.FromString<List<SavedCookbooksData>>(savedCookbooksData);
			var userSavedCookbookIds = savedCookbooks?.FirstOrDefault(x => x.UserId == Guid.Parse(userId))?.SavedCookbooks ?? new List<Guid>();

			var userSavedCookbooks = cookbooks?.Where(cb => userSavedCookbookIds.Contains(cb.Id)).ToList();
			return serializer.ToString(userSavedCookbooks);
		}

		//Creating a new cookbook
		if (request.RequestUri.AbsolutePath == "/api/cookbook" && request.Method == HttpMethod.Post)
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
		if (request.RequestUri.AbsolutePath == "/api/cookbook" && request.Method == HttpMethod.Put)
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
