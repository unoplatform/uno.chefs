namespace Chefs.Services.Notifications;

public class NotificationService : INotificationService
{
	private readonly INotificationEndpoint _notificationEndpoint;

	public NotificationService(INotificationEndpoint notificationEndpoint)
		=> _notificationEndpoint = notificationEndpoint;

	public async ValueTask<IImmutableList<Notification>> GetAll(CancellationToken ct)
		=> (await _notificationEndpoint.GetAll(ct))
			.Select(n => new Notification(n))
			.ToImmutableList();

	public async ValueTask<IImmutableList<Notification>> GetRead(CancellationToken ct)
		=> (await _notificationEndpoint.GetRead(ct))
			.Select(n => new Notification(n))
			.ToImmutableList();

	public async ValueTask<IImmutableList<Notification>> GetUnread(CancellationToken ct)
		=> (await _notificationEndpoint.GetUnread(ct))
			.Select(n => new Notification(n))
			.ToImmutableList();
}
