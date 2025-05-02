namespace Chefs.Services.Notifications;

/// <summary>
/// Implements notification related methods
/// </summary>
public interface INotificationService
{
	/// <summary>
	/// Gets all notifications from api
	/// </summary>
	/// <param name="ct"></param>
	/// <returns>
	/// All notifications
	/// </returns>
	ValueTask<IImmutableList<Notification>> GetAll(CancellationToken ct);
}
