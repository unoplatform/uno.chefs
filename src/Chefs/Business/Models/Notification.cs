using Chefs.Data;

namespace Chefs.Business;

public record Notification
{
    internal Notification(NotificationData notificationData)
    {
        Title = notificationData.Title;
        Description = notificationData.Description;
        Read = notificationData.Read;
        Date = notificationData.Date;
    }

    public string? Title { get; init; }
    public string? Description { get; init; }
    public bool Read { get; init; }
    public DateTime Date { get; init; }
}
