using System.Collections.Immutable;

namespace Chefs.Business;

public interface INotificationService
{
    ValueTask<IImmutableList<Notification>> GetAll();
}
