namespace Chefs.Client.Mock;

public class MockUserEndpoints(string basePath, ISerializer serializer, ILogger<BaseMockEndpoint> logger) : BaseMockEndpoint(serializer, logger)
{
	public async Task<string> HandleUsersRequest(HttpRequestMessage request)
	{
		var users = await LoadData<List<UserData>>("Users.json") ?? [];

		//authenticate user
		if (request.RequestUri.AbsolutePath.Contains("/api/User/authenticate") && request.Method == HttpMethod.Post)
		{
			var loginRequest = serializer.FromString<LoginRequest>(request.Content.ReadAsStringAsync().Result);
			var user = users?.FirstOrDefault(u => u.Email == loginRequest?.Email && u.Password == loginRequest.Password);
			if (user != null)
			{
				return serializer.ToString(user.Id);
			}
			return "Unauthorized";
		}
		//Get popular creators
		if (request.RequestUri.AbsolutePath.Contains("/api/User/popular-creators"))
		{
			var popularCreators = users?.Where(u => u.Id != Guid.Parse("3c896419-e280-40e7-8552-240635566fed")).ToList();
			return serializer.ToString(popularCreators);
		}
		//Get current user
		if (request.RequestUri.AbsolutePath.Contains("/api/User/current"))
		{
			var currentUser = users?.FirstOrDefault(u => u.Id == Guid.Parse("3c896419-e280-40e7-8552-240635566fed"));
			if (currentUser != null)
			{
				currentUser.IsCurrent = true;
				return serializer.ToString(currentUser);
			}
			return "NotFound";
		}

		if (Guid.TryParse(request.RequestUri.Segments.Last(), out var userId))
		{
			var user = users?.FirstOrDefault(u => u.Id == userId);
			if (user != null)
			{
				return serializer.ToString(user);
			}
			return "NotFound";
		}

		return serializer.ToString(users);
	}

}
