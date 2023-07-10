namespace Chefs.Presentation;
public partial class NotificationsModel
{
	private readonly INotificationService _notificationService;

	public NotificationsModel(INotificationService notificationService)
	{
		_notificationService = notificationService;
	}
	public IFeed<GroupedNotification> Notifications => Feed<GroupedNotification>.Async(async ct
			=> await _notificationService.GetAll(ct) is { Count: > 0 } result
			? new GroupedNotification(result)
			: Option.None<GroupedNotification>());

	public IFeed<GroupedNotification> Unread => Feed<GroupedNotification>.Async(async ct
			=> await _notificationService.GetUnread(ct) is { Count: > 0 } result
			? new GroupedNotification(result)
			: Option.None<GroupedNotification>());

	public IFeed<GroupedNotification> Read => Feed<GroupedNotification>.Async(async ct
			=> await _notificationService.GetRead(ct) is { Count: > 0 } result
			? new GroupedNotification(result.Where(x => x.Read))
			: Option.None<GroupedNotification>());
}
