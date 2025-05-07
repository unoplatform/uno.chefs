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
	public IActionResult GetAll()
	{
		var notifications = LoadData<List<NotificationData>>(_notificationsFilePath);
		return Ok(notifications.ToImmutableList());
	}
}
