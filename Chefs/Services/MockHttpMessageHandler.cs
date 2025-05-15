using System.Net;
using System.Text;

namespace Chefs.Services;

public class MockHttpMessageHandler : HttpMessageHandler
{
	private readonly string _basePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "AppData");

	private readonly MockRecipeEndpoints _mockRecipeEndpoints;
	private readonly MockUserEndpoints _mockUserEndpoints;
	private readonly MockCookbookEndpoints _mockCookbookEndpoints;
	private readonly MockNotificationEndpoints _mockNotificationEndpoints;

	public MockHttpMessageHandler(ISerializer serializer, ILogger<BaseMockEndpoint> logger)
	{
		_mockRecipeEndpoints = new MockRecipeEndpoints(_basePath, serializer, logger);
		_mockUserEndpoints = new MockUserEndpoints(_basePath, serializer, logger);
		_mockCookbookEndpoints = new MockCookbookEndpoints(_basePath, serializer, logger);
		_mockNotificationEndpoints = new MockNotificationEndpoints(_basePath, serializer, logger);
	}

	protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
		{
			Content = new StringContent(await GetMockData(request), Encoding.UTF8, "application/json")
		};

		return await Task.FromResult(mockResponse);
	}

	private async Task<string> GetMockData(HttpRequestMessage request)
	{
		// Handle Recipes
		if (request.RequestUri.AbsolutePath.Contains("/api/Recipe"))
		{
			return await _mockRecipeEndpoints.HandleRecipesRequest(request);
		}

		// Handle Users
		if (request.RequestUri.AbsolutePath.Contains("/api/User"))
		{
			return await _mockUserEndpoints.HandleUsersRequest(request);
		}

		// Handle Cookbooks
		if (request.RequestUri.AbsolutePath.Contains("/api/Cookbook"))
		{
			return await _mockCookbookEndpoints.HandleCookbooksRequest(request);
		}

		// Handle Notifications
		if (request.RequestUri.AbsolutePath.Contains("/api/Notification"))
		{
			return await _mockNotificationEndpoints.HandleNotificationsRequest(request);
		}

		return "{}";
	}
}
