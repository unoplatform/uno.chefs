namespace Chefs.Services.Endpoints;

public interface INotificationEndpoint
{
	ValueTask<IImmutableList<NotificationData>> GetAll(CancellationToken ct);
	ValueTask<IImmutableList<NotificationData>> GetRead(CancellationToken ct);
	ValueTask<IImmutableList<NotificationData>> GetUnread(CancellationToken ct);
}
