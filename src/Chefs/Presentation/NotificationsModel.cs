namespace Chefs.Presentation;
public partial class NotificationsModel
{
	private readonly INavigator _navigator;
	private readonly INotificationService _notificationService;

	public NotificationsModel(INavigator navigator, INotificationService notificationService)
	{
		_notificationService = notificationService;
		_navigator = navigator;
	}
	public IFeed<GroupedNotification> Notifications => Feed<GroupedNotification>.Async(async ct
			=> await _notificationService.GetAll(ct) is { Count: > 0 } result
			? new GroupedNotification(result)
			: Option.None<GroupedNotification>());

	public IFeed<GroupedNotification> Unread => Notifications.Select(group =>
		new GroupedNotification(group.GetAll().Where(n => !n.Read).ToImmutableList()));

	public IFeed<GroupedNotification> Read => Notifications.Select(group =>
		new GroupedNotification(group.GetAll().Where(n => n.Read).ToImmutableList()));

	public async Task CloseNotificationPage()
	{
		await NavigateBack();
	}

	private async Task NavigateBack()
	{
		await _navigator.NavigateBackAsync(this);
	}
}
