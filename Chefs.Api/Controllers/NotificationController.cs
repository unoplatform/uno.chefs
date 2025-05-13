namespace Chefs.Api.Controllers;

/// <summary>
/// Notification Endpoints
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class NotificationController() : ChefsControllerBase
{
	private readonly string _notificationsFilePath = "Notifications.json";

	/// <summary>
	/// Retrieves all notifications.
	/// </summary>
	/// <returns>A list of notifications.</returns>
	[HttpGet]
	[Produces("application/json")]
	[ProducesResponseType(typeof(IEnumerable<NotificationData>), 200)]
	[ProducesResponseType(404)]
	public ActionResult<IEnumerable<NotificationData>> GetAll()
	{
		var notifications = LoadData<List<NotificationData>>(_notificationsFilePath);
		return Ok(notifications.ToImmutableList());
	}
}
