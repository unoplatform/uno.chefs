namespace Chefs.Api.Controllers;

/// <summary>
/// Cookbook Endpoints
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CookbookController() : ChefsControllerBase
{
	private readonly string _cookbooksFilePath = "Cookbooks.json";
	private readonly string _savedCookbooksFilePath = "SavedCookbooks.json";

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
	public IActionResult Save([FromBody] CookbookData cookbook, [FromQuery] Guid userId) =>
		// We do not persist the saved state in this example.
		NoContent();

	/// <summary>
	/// Retrieves saved cookbooks for a specific user.
	/// </summary>
	/// <param name="userId">The user ID.</param>
	/// <returns>A list of saved cookbooks.</returns>
	[HttpGet("saved")]
	public IActionResult GetSaved([FromQuery] Guid userId)
	{
		var savedCookbooks = LoadData<List<Guid>>(_savedCookbooksFilePath);

		var cookbooks = LoadData<List<CookbookData>>(_cookbooksFilePath);
		var savedCookbooksList = cookbooks.Where(cb => savedCookbooks.Contains(cb.Id)).ToImmutableList();

		return Ok(savedCookbooksList);
	}
}
