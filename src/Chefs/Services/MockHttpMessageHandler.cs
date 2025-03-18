using System.Net;
using System.Text;
using System.Text.Json;

namespace Chefs.Services;

public class MockHttpMessageHandler : HttpMessageHandler
{
	private readonly string _basePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "AppData");

	private readonly MockRecipeEndpoints _mockRecipeEndpoints;
	private readonly MockUserEndpoints _mockUserEndpoints;
	private readonly MockCookbookEndpoints _mockCookbookEndpoints;
	private readonly MockNotificationEndpoints _mockNotificationEndpoints;

	public MockHttpMessageHandler(ISerializer serializer)
	{
		_mockRecipeEndpoints = new MockRecipeEndpoints(_basePath, serializer);
		_mockUserEndpoints = new MockUserEndpoints(_basePath,serializer);
		_mockCookbookEndpoints = new MockCookbookEndpoints(_basePath, serializer);
		_mockNotificationEndpoints = new MockNotificationEndpoints(_basePath, serializer);
	}

	protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
		{
			Content = new StringContent(GetMockData(request), Encoding.UTF8, "application/json")
		};

		return await Task.FromResult(mockResponse);
	}

	private string GetMockData(HttpRequestMessage request)
	{
		// Handle Recipes
		if (request.RequestUri.AbsolutePath.Contains("/api/recipe"))
		{
			return _mockRecipeEndpoints.HandleRecipesRequest(request);
		}

		// Handle Users
		if (request.RequestUri.AbsolutePath.Contains("/api/user"))
		{
			return _mockUserEndpoints.HandleUsersRequest(request);
		}

		// Handle Cookbooks
		if (request.RequestUri.AbsolutePath.Contains("/api/cookbook"))
		{
			return _mockCookbookEndpoints.HandleCookbooksRequest(request);
		}

		// Handle Notifications
		if (request.RequestUri.AbsolutePath.Contains("/api/notification"))
		{
			return _mockNotificationEndpoints.HandleNotificationsRequest(request);
		}

		return "{}";
	}
}
