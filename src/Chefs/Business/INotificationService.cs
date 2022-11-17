namespace Chefs.Business;

/// <summary>
/// Implements notification related methods
/// </summary>
public interface INotificationService
{
    /// <summary>
    /// Notifications method
    /// </summary>
    /// <param name="ct"></param>
    /// <returns>
    /// Get each notifiacion from api
    /// </returns>
    ValueTask<IImmutableList<Notification>> GetAll(CancellationToken ct);
}
