using System.Text.Json;
using Chefs.Services.Clients.Models;
using UserData = Chefs.Data.UserData;

namespace Chefs.Services;

public class MockUserEndpoints(string basePath, JsonSerializerOptions serializerOptions)
{
	public string HandleUsersRequest(HttpRequestMessage request)
	{
		var usersData = File.ReadAllText(Path.Combine(basePath, "Users.json"));
		var users = JsonSerializer.Deserialize<List<UserData>>(usersData, serializerOptions);

		//authenticate user
		if (request.RequestUri.AbsolutePath.Contains("/api/user/authenticate") && request.Method == HttpMethod.Post)
		{
			var loginRequest = JsonSerializer.Deserialize<LoginRequest>(request.Content.ReadAsStringAsync().Result, serializerOptions);
			var user = users?.FirstOrDefault(u => u.Email == loginRequest?.Email && u.Password == loginRequest.Password);
			if (user != null)
			{
				return JsonSerializer.Serialize(user.Id, serializerOptions);
			}
			return "Unauthorized";
		}
		//Get popular creators
		if (request.RequestUri.AbsolutePath.Contains("/api/user/popular-creators"))
		{
			var popularCreators = users?.Where(u => u.Id != Guid.Parse("3c896419-e280-40e7-8552-240635566fed")).ToList();
			return JsonSerializer.Serialize(popularCreators, serializerOptions);
		}
		//Get current user
		if (request.RequestUri.AbsolutePath.Contains("/api/user/current"))
		{
			var currentUser = users?.FirstOrDefault(u => u.Id == Guid.Parse("3c896419-e280-40e7-8552-240635566fed"));
			if (currentUser != null)
			{
				currentUser.IsCurrent = true;
				return JsonSerializer.Serialize(currentUser, serializerOptions);
			}
			return "NotFound";
		}

		if (Guid.TryParse(request.RequestUri.Segments.Last(), out var userId))
		{
			var user = users?.FirstOrDefault(u => u.Id == userId);
			if (user != null)
			{
				return JsonSerializer.Serialize(user, serializerOptions);
			}
			return "NotFound";
		}

		return JsonSerializer.Serialize(users, serializerOptions);
	}

}
