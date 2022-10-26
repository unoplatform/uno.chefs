using System.Collections.Immutable;
using Uno.Extensions.Serialization;
using Uno.Extensions.Storage;

namespace Chefs.Data;

public class NotificationEndpoint : INotificationEndpoint
{
    public const string NotifacationDataFile = "notifications.json";

    private readonly IStorage _dataService;
    private readonly ISerializer _serializer;

    public NotificationEndpoint(IStorage dataService, ISerializer serializer)
        => (_dataService, _serializer) = (dataService, serializer);

    public async ValueTask<IImmutableList<NotificationData>> GetAll(CancellationToken ct) => await _dataService
        .ReadFileAsync<IImmutableList<NotificationData>>(_serializer, NotifacationDataFile) 
        ?? ImmutableList<NotificationData>.Empty;
}
