using Chefs.Services.Clients;

namespace Chefs.Services.Notifications;

public class NotificationService(ChefsApiClient client) : INotificationService
{
	public async ValueTask<IImmutableList<Notification>> GetAll(CancellationToken ct)
	{
		var notificationData =
			await client.Api.Notification.GetAsync(cancellationToken: ct).ConfigureAwait(false) ?? [];

		return notificationData.Select(notification => new Notification(notification)).ToImmutableList();
	}
}
