using NotificationData = Chefs.Services.Clients.Models.NotificationData;

namespace Chefs.Business.Models;

public record Notification
{
	internal Notification(NotificationData notificationData)
	{
		Title = notificationData.Title;
		Description = notificationData.Description;
		Read = notificationData.IsRead ?? false;
		Date = notificationData.Date?.DateTime ?? DateTime.MinValue;
	}
	
	public string? Title { get; init; }
	public string? Description { get; init; }
	public bool Read { get; init; }
	public DateTime Date { get; init; }
}
