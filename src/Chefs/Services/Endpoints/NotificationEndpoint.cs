namespace Chefs.Services.Endpoints;

public class NotificationEndpoint : INotificationEndpoint
{
	public const string NotificationDataFile = "Notifications.json";

	private readonly IStorage _dataService;
	private readonly ISerializer _serializer;

	public NotificationEndpoint(IStorage dataService, ISerializer serializer)
		=> (_dataService, _serializer) = (dataService, serializer);

	public async ValueTask<IImmutableList<NotificationData>> GetAll(CancellationToken ct)
		=> await _dataService.ReadPackageFileAsync<IImmutableList<NotificationData>>(_serializer, NotificationDataFile)
		?? ImmutableList<NotificationData>.Empty;
}
