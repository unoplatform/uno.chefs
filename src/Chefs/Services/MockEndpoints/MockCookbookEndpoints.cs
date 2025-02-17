using System.Text.Json;

namespace Chefs.Services;

public class MockCookbookEndpoints(string basePath, JsonSerializerOptions serializerOptions) : BaseMockEndpoint
{
	public string HandleCookbooksRequest(HttpRequestMessage request)
	{
		var cookbooksData = LoadData("Cookbooks.json");
		var cookbooks = JsonSerializer.Deserialize<List<CookbookData>>(cookbooksData, serializerOptions);

		if (request.RequestUri.AbsolutePath == "/api/cookbook")
		{
			return JsonSerializer.Serialize(cookbooks, serializerOptions);
		}

		//Retrieving saved cookbooks for a user
		if (request.RequestUri.AbsolutePath.Contains("/api/cookbook/saved") && request.Method == HttpMethod.Get)
		{
			var queryParams = request.RequestUri.Query;
			var userId = ExtractUserIdFromQuery(queryParams);
			var savedCookbooksData = LoadData("SavedCookbooks.json");
			var savedCookbooks = JsonSerializer.Deserialize<List<SavedCookbooksData>>(savedCookbooksData, serializerOptions);
			var userSavedCookbookIds = savedCookbooks?.FirstOrDefault(x => x.UserId == Guid.Parse(userId))?.SavedCookbooks ?? new List<Guid>();

			var userSavedCookbooks = cookbooks?.Where(cb => userSavedCookbookIds.Contains(cb.Id)).ToList();
			return JsonSerializer.Serialize(userSavedCookbooks, serializerOptions);
		}

		//Creating a new cookbook
		if (request.RequestUri.AbsolutePath == "/api/cookbook" && request.Method == HttpMethod.Post)
		{
			var cookbook = JsonSerializer.Deserialize<CookbookData>(request.Content.ReadAsStringAsync().Result, serializerOptions);
			var queryParams = request.RequestUri.Query;
			var userId = ExtractUserIdFromQuery(queryParams);
			cookbook.UserId = Guid.Parse(userId);

			cookbooks?.Add(cookbook);
			File.WriteAllText(Path.Combine(basePath, "Cookbooks.json"), JsonSerializer.Serialize(cookbooks, serializerOptions));

			return JsonSerializer.Serialize(cookbook, serializerOptions);
		}

		//Updating a cookbook
		if (request.RequestUri.AbsolutePath == "/api/cookbook" && request.Method == HttpMethod.Put)
		{
			var updatedCookbook = JsonSerializer.Deserialize<CookbookData>(request.Content.ReadAsStringAsync().Result, serializerOptions);
			var cookbookItem = cookbooks?.FirstOrDefault(c => updatedCookbook != null && c.Id == updatedCookbook.Id);

			if (cookbookItem != null)
			{
				cookbookItem.Name = updatedCookbook?.Name;
				cookbookItem.Recipes = updatedCookbook?.Recipes;
				File.WriteAllText(Path.Combine(basePath, "Cookbooks.json"), JsonSerializer.Serialize(cookbooks, serializerOptions));
				return JsonSerializer.Serialize(cookbookItem, serializerOptions);
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
