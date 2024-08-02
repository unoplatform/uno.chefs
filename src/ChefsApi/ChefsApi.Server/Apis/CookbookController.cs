using System.Collections.Immutable;
using System.Text.Json;
using Chefs.Data;

namespace ChefsApi.Server.Apis;

/// <summary>
/// Cookbook Endpoints
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CookbookController : ControllerBase
{
    private readonly string _cookbooksFilePath = "Data/AppData/Cookbooks.json";
    private readonly string _savedCookbooksFilePath = "Data/AppData/SavedCookbooks.json";

    /// <summary>
    /// Retrieves all cookbooks.
    /// </summary>
    /// <returns>A list of cookbooks.</returns>
    [HttpGet]
    public IActionResult GetAll()
    {
        var cookbooks = LoadData<List<CookbookData>>(_cookbooksFilePath);
        return Ok(cookbooks.ToImmutableList());
    }

    /// <summary>
    /// Creates a new cookbook.
    /// </summary>
    /// <param name="cookbook">The cookbook data.</param>
    /// <param name="userId">The user ID.</param>
    /// <returns>The created cookbook.</returns>
    [HttpPost]
    public IActionResult Create([FromBody] CookbookData cookbook, [FromQuery] Guid userId)
    {
        var cookbooks = LoadData<List<CookbookData>>(_cookbooksFilePath);
        cookbook.UserId = userId;
        cookbooks.Add(cookbook);

        System.IO.File.WriteAllText(_cookbooksFilePath, JsonSerializer.Serialize(cookbooks));

        return Created("", cookbook);
    }

    /// <summary>
    /// Updates an existing cookbook.
    /// </summary>
    /// <param name="cookbook">The updated cookbook data.</param>
    /// <returns>The updated cookbook, or NotFound if the cookbook does not exist.</returns>
    [HttpPut]
    public IActionResult Update([FromBody] CookbookData cookbook)
    {
        var cookbooks = LoadData<List<CookbookData>>(_cookbooksFilePath);
        var cookbookItem = cookbooks.FirstOrDefault(c => c.Id == cookbook.Id);

        if (cookbookItem != null)
        {
            cookbookItem.Name = cookbook.Name;
            cookbookItem.Recipes = cookbook.Recipes;

            System.IO.File.WriteAllText(_cookbooksFilePath, JsonSerializer.Serialize(cookbooks));

            return Ok(cookbookItem);
        }
        else
        {
            return NotFound("Cookbook not found");
        }
    }

    /// <summary>
    /// Saves or unsaves a cookbook for a specific user.
    /// </summary>
    /// <param name="cookbook">The cookbook data.</param>
    /// <param name="userId">The user ID.</param>
    /// <returns>No content.</returns>
    [HttpPost("save")]
    public IActionResult Save([FromBody] CookbookData cookbook, [FromQuery] Guid userId)
    {
        var savedCookbooks = LoadData<List<SavedCookbooksData>>(_savedCookbooksFilePath);
        var userSavedCookbooks = savedCookbooks.FirstOrDefault(x => x.UserId == userId);

        if (userSavedCookbooks != null)
        {
            if (userSavedCookbooks.SavedCookbooks.Contains(cookbook.Id))
            {
                userSavedCookbooks.SavedCookbooks = userSavedCookbooks.SavedCookbooks.Where(id => id != cookbook.Id).ToList();
            }
            else
            {
                userSavedCookbooks.SavedCookbooks.Add(cookbook.Id);
            }
        }
        else
        {
            savedCookbooks.Add(new SavedCookbooksData { UserId = userId, SavedCookbooks = new List<Guid> { cookbook.Id } });
        }

        System.IO.File.WriteAllText(_savedCookbooksFilePath, JsonSerializer.Serialize(savedCookbooks));

        return NoContent();
    }

    /// <summary>
    /// Retrieves saved cookbooks for a specific user.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <returns>A list of saved cookbooks.</returns>
    [HttpGet("saved")]
    public IActionResult GetSaved([FromQuery] Guid userId)
    {
        var savedCookbooks = LoadData<List<SavedCookbooksData>>(_savedCookbooksFilePath);
        var userSavedCookbookIds = savedCookbooks.FirstOrDefault(x => x.UserId == userId)?.SavedCookbooks ?? new List<Guid>();

        var cookbooks = LoadData<List<CookbookData>>(_cookbooksFilePath);
        var savedCookbooksList = cookbooks.Where(cb => userSavedCookbookIds.Contains(cb.Id)).ToImmutableList();

        return Ok(savedCookbooksList);
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
