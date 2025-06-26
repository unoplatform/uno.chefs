using NotificationData = Chefs.Client.Models.NotificationData;

namespace Chefs.Business.Models;

public record Notification
{
	internal Notification(NotificationData notificationData)
	{
		Title = notificationData.Title;
		Description = notificationData.Description;
		IsRead = notificationData.IsRead ?? false;
		Date = notificationData.Date?.DateTime ?? DateTime.MinValue;
	}

	public string? Title { get; init; }
	public string? Description { get; init; }
	public bool IsRead { get; init; }
	public DateTime Date { get; init; }
}
