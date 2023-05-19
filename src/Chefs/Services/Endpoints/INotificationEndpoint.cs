namespace Chefs.Services.Endpoints;

public interface INotificationEndpoint
{
	ValueTask<IImmutableList<NotificationData>> GetAll(CancellationToken ct);
}
