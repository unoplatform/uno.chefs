using Chefs.Client;

namespace Chefs.Business.Services.Notifications;

public class NotificationService(ChefsApiClient client, ILogger<NotificationService> logger) : INotificationService
{
	public async ValueTask<IImmutableList<Notification>> GetAll(CancellationToken ct)
	{
		try
		{
			return await GetAllCore(ct);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Failed to get all notifications");
			throw;
		}
	}

	private async ValueTask<IImmutableList<Notification>> GetAllCore(CancellationToken ct)
	{
		var notificationsData = await client.Api.Notification.GetAsync(cancellationToken: ct);
		return notificationsData?.Select(n => new Notification(n)).ToImmutableList() ?? ImmutableList<Notification>.Empty;
	}
}
