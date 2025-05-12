namespace Chefs.Api.Controllers;

/// <summary>
/// Notification Endpoints
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class NotificationController(IWebHostEnvironment env) : ControllerBase
{
	private readonly string _notificationsFilePath = "Notifications.json";
	private readonly string _appDataPath = Path.Combine(env.ContentRootPath, "AppData");

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

	/// <summary>
	/// Loads data from a specified JSON file.
	/// </summary>
	/// <typeparam name="T">The type of data to load.</typeparam>
	/// <param name="fileName">The file name of the JSON file.</param>
	/// <returns>The loaded data.</returns>
	private T? LoadData<T>(string fileName)
	{
		var json = System.IO.File.ReadAllText(Path.Combine(_appDataPath, fileName));
		return JsonSerializer.Deserialize<T>(json);
	}
}
