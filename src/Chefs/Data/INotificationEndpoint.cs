using System.Collections.Immutable;

namespace Chefs.Data;

public interface INotificationEndpoint
{
    ValueTask<IImmutableList<NotificationData>> GetAll(CancellationToken ct);
}
