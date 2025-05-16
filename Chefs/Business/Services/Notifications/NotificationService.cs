using Chefs.Client;

namespace Chefs.Business.Services.Notifications;

public class NotificationService(ChefsApiClient client) : INotificationService
{
	public async ValueTask<IImmutableList<Notification>> GetAll(CancellationToken ct)
	{
		var notificationsData = await client.Api.Notification.GetAsync(cancellationToken: ct);
		return notificationsData?.Select(n => new Notification(n)).ToImmutableList() ?? ImmutableList<Notification>.Empty;
	}
}
