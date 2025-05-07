
namespace Chefs.Services;

public class MockNotificationEndpoints(string basePath, ISerializer serializer) : BaseMockEndpoint
{
	public string HandleNotificationsRequest(HttpRequestMessage request)
	{
		var notificationsData = LoadData("Notifications.json");
		var notifications = serializer.FromString<List<NotificationData>>(notificationsData);
		
		//Get all notifications
		if (request.RequestUri.AbsolutePath == "/api/notification" && request.Method == HttpMethod.Get)
		{
			return serializer.ToString(notifications);
		}
		
		return "{}";
	}
}
