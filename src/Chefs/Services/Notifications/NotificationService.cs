using Chefs.Services.Clients;
using Microsoft.Kiota.Abstractions.Serialization;
using NotificationData = Chefs.Services.Clients.Models.NotificationData;

namespace Chefs.Services.Notifications;

public class NotificationService(ChefsApiClient client) : INotificationService
{
	public async ValueTask<IImmutableList<Notification>> GetAll(CancellationToken ct)
	{
		await using var responseStream = await client.Api.Notification.GetAsync(cancellationToken: ct);
		var jsonResponse = await new StreamReader(responseStream).ReadToEndAsync(ct);
		var notificationsData = await KiotaJsonSerializer.DeserializeCollectionAsync<NotificationData>(jsonResponse, cancellationToken: ct);
		return notificationsData?.Select(n => new Notification(n)).ToImmutableList() ?? ImmutableList<Notification>.Empty;
	}
}
