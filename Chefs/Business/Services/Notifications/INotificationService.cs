namespace Chefs.Business.Services.Notifications;

/// <summary>
/// Implements notification related methods
/// </summary>
public interface INotificationService
{
	/// <summary>
	/// Gets all notifications from the API.
	/// </summary>
	/// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
	/// <returns>All notifications.</returns>
	ValueTask<IImmutableList<Notification>> GetAll(CancellationToken ct);
}
