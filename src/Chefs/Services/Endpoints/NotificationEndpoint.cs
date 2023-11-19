namespace Chefs.Services.Endpoints;

public class NotificationEndpoint : INotificationEndpoint
{
	private readonly IStorage _dataService;
	private readonly ISerializer _serializer;
	private List<NotificationData>? _notifications;

	public NotificationEndpoint(IStorage dataService, ISerializer serializer)
		=> (_dataService, _serializer) = (dataService, serializer);

	public async ValueTask<IImmutableList<NotificationData>> GetAll(CancellationToken ct)
		=> (await Load(ct)).ToImmutableList() ?? ImmutableList<NotificationData>.Empty;

	public async ValueTask<IImmutableList<NotificationData>> GetRead(CancellationToken ct) => (await Load(ct))
		.Where(x => x.Read)
		.ToImmutableList() ?? ImmutableList<NotificationData>.Empty;

	public async ValueTask<IImmutableList<NotificationData>> GetUnread(CancellationToken ct) => (await Load(ct))
		.Where(x => !x.Read)
		.ToImmutableList() ?? ImmutableList<NotificationData>.Empty;

	private async ValueTask<IList<NotificationData>> Load(CancellationToken ct)
	{
		if (_notifications == null)
		{
			_notifications = await _dataService.ReadPackageFileAsync<List<NotificationData>>(_serializer, Constants.NotificationsDataFile)
								?? new List<NotificationData>();
		}

		return _notifications ?? new List<NotificationData>();
	}
}
