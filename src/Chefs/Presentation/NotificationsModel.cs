namespace Chefs.Presentation;

public partial class NotificationsModel
{
	private readonly INotificationService _notificationService;

	public NotificationsModel(INotificationService notificationService)
	{
		_notificationService = notificationService;
	}

	public IFeed<GroupedNotification> Notifications => Feed<GroupedNotification>.Async(async ct => new GroupedNotification(await _notificationService.GetAll(ct)));

	public IFeed<GroupedNotification> Unread => Notifications.Select(x => new GroupedNotification(x.GetAll().Where(y => !y.Read)));

	public IFeed<GroupedNotification> Read => Notifications.Select(x => new GroupedNotification(x.GetAll().Where(y => y.Read)));
}
