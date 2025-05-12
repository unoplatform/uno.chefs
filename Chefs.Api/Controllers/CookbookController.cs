using Chefs.DataContracts;

namespace Chefs.Api.Controllers;

/// <summary>
/// Cookbook Endpoints
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CookbookController(IWebHostEnvironment env) : ControllerBase
{
	private readonly string _cookbooksFilePath = "Cookbooks.json";
	private readonly string _savedCookbooksFilePath = "SavedCookbooks.json";
	private readonly string _appDataPath = Path.Combine(env.ContentRootPath, "AppData");

	/// <summary>
	/// Retrieves all cookbooks.
	/// </summary>
	/// <returns>A list of cookbooks.</returns>
	[HttpGet]
	[Produces("application/json")]
	[ProducesResponseType(typeof(IEnumerable<CookbookData>), 200)]
	[ProducesResponseType(404)]
	public ActionResult<IEnumerable<CookbookData>> GetAll()
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
	[Produces("application/json")]
	[ProducesResponseType(typeof(CookbookData), 201)]
	public ActionResult<CookbookData> Create([FromBody] CookbookData cookbook, [FromQuery] Guid userId)
	{
		var cookbooks = LoadData<List<CookbookData>>(_cookbooksFilePath);
		cookbook.UserId = userId;
		cookbooks.Add(cookbook);

		return Created("", cookbook);
	}

	/// <summary>
	/// Updates an existing cookbook.
	/// </summary>
	/// <param name="cookbook">The updated cookbook data.</param>
	/// <returns>The updated cookbook, or NotFound if the cookbook does not exist.</returns>
	[HttpPut]
	[Produces("application/json")]
	[ProducesResponseType(typeof(CookbookData), 200)]
	[ProducesResponseType(404)]
	public ActionResult<CookbookData> Update([FromBody] CookbookData cookbook)
	{
		var cookbooks = LoadData<List<CookbookData>>(_cookbooksFilePath);
		var cookbookItem = cookbooks.FirstOrDefault(c => c.Id == cookbook.Id);

		if (cookbookItem != null)
		{
			cookbookItem.Name = cookbook.Name;
			cookbookItem.Recipes = cookbook.Recipes;

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
	[ProducesResponseType(204)]
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

		return NoContent();
	}

	/// <summary>
	/// Retrieves saved cookbooks for a specific user.
	/// </summary>
	/// <param name="userId">The user ID.</param>
	/// <returns>A list of saved cookbooks.</returns>
	[HttpGet("saved")]
	[Produces("application/json")]
	[ProducesResponseType(typeof(IEnumerable<CookbookData>), 200)]
	public ActionResult<IEnumerable<CookbookData>> GetSaved([FromQuery] Guid userId)
	{
		var savedCookbooks = LoadData<List<SavedCookbooksData>>(_savedCookbooksFilePath);
		var userSavedCookbookIds = savedCookbooks.FirstOrDefault(x => x.UserId == userId)?.SavedCookbooks ?? [];

		var cookbooks = LoadData<List<CookbookData>>(_cookbooksFilePath);
		var savedCookbooksList = cookbooks.Where(cb => userSavedCookbookIds.Contains(cb.Id)).ToImmutableList();

		return Ok(savedCookbooksList);
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
