namespace Chefs.Client.Mock;

public class MockNotificationEndpoints(string basePath, ISerializer serializer, ILogger<BaseMockEndpoint> logger) : BaseMockEndpoint(serializer, logger)
{
	public async Task<string> HandleNotificationsRequest(HttpRequestMessage request)
	{
		var notifications = await LoadData<List<NotificationData>>("Notifications.json")
							?? [];

		//Get all notifications
		if (request.RequestUri.AbsolutePath == "/api/Notification" && request.Method == HttpMethod.Get)
		{
			return serializer.ToString(notifications);
		}

		return "{}";
	}
}
