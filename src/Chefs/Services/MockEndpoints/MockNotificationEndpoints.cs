using System.Text.Json;

namespace Chefs.Services;

public class MockNotificationEndpoints(string basePath, JsonSerializerOptions serializerOptions) : BaseMockEndpoint
{
	public string HandleNotificationsRequest(HttpRequestMessage request)
	{
		var notificationsData = LoadData("Notifications.json");
		var notifications = JsonSerializer.Deserialize<List<NotificationData>>(notificationsData, serializerOptions);
		
		//Get all notifications
		if (request.RequestUri.AbsolutePath == "/api/notification" && request.Method == HttpMethod.Get)
		{
			return JsonSerializer.Serialize(notifications, serializerOptions);
		}
		
		return "{}";
	}
}
