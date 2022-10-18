using System.Collections.Immutable;

namespace Chefs.Business;

public class NotificationService : INotificationService
{
    public ValueTask<IImmutableList<Notification>> GetAll()
    {
        throw new NotImplementedException();
    }
}
