using System.Collections.Immutable;
using System.Text.Json;
using Chefs.Data;

namespace ChefsApi.Server.Apis;

/// <summary>
/// Notification Endpoints
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class NotificationController : ControllerBase
{
    private readonly string _notificationsFilePath = "Data/AppData/Notifications.json";
    
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
    /// <param name="filePath">The file path of the JSON file.</param>
    /// <returns>The loaded data.</returns>
    private T LoadData<T>(string filePath)
    {
        var json = System.IO.File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<T>(json);
    }
}
