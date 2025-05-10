using Chefs.DataContracts;

namespace Chefs.Api.Controllers;

/// <summary>
/// Notification Endpoints
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class NotificationController : ControllerBase
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

	/// <summary>
	/// Loads data from a specified JSON file.
	/// </summary>
	/// <typeparam name="T">The type of data to load.</typeparam>
	/// <param name="fileName">The file name of the JSON file.</param>
	/// <returns>The loaded data.</returns>
	private T LoadData<T>(string fileName)
	{
		var json = EmbeddedJsonLoader.Load(fileName);
		return JsonSerializer.Deserialize<T>(json);
	}
}
