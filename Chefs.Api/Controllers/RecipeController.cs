namespace Chefs.Api.Controllers;

/// <summary>
/// Recipe Endpoints
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class RecipeController() : ChefsControllerBase
{
	private readonly string _recipesFilePath = "Recipes.json";
	private readonly string _savedRecipesFilePath = "SavedRecipes.json";
	private readonly string _categoriesFilePath = "categories.json";

	/// <summary>
	/// Retrieves all recipes.
	/// </summary>
	/// <returns>A list of recipes.</returns>
	[HttpGet]
	[Produces("application/json")]
	[ProducesResponseType(typeof(IEnumerable<RecipeData>), 200)]
	[ProducesResponseType(404)]
	public ActionResult<IEnumerable<RecipeData>> GetAll()
	{
		var recipes = LoadData<List<RecipeData>>(_recipesFilePath);
		return Ok(recipes.ToImmutableList());
	}

	/// <summary>
	/// Retrieves the count of recipes for a specific user.
	/// </summary>
	/// <param name="userId">The user ID.</param>
	/// <returns>The count of recipes for the user.</returns>
	[HttpGet("count")]
	[Produces("application/json")]
	[ProducesResponseType(typeof(int), 200)]
	public ActionResult<int> GetCount([FromQuery] Guid userId)
	{
		var recipes = LoadData<List<RecipeData>>(_recipesFilePath);
		var count = recipes.Count(r => r.UserId == userId);
		return Ok(count);
	}

	/// <summary>
	/// Retrieves all recipe categories.
	/// </summary>
	/// <returns>A list of categories.</returns>
	[HttpGet("categories")]
	[Produces("application/json")]
	[ProducesResponseType(typeof(IEnumerable<CategoryData>), 200)]
	[ProducesResponseType(404)]
	public ActionResult<IEnumerable<CategoryData>> GetCategories()
	{
		var categories = LoadData<List<CategoryData>>(_categoriesFilePath);
		return Ok(categories.ToImmutableList());
	}

	/// <summary>
	/// Retrieves trending recipes.
	/// </summary>
	/// <returns>A list of trending recipes.</returns>
	[HttpGet("trending")]
	[Produces("application/json")]
	[ProducesResponseType(typeof(IEnumerable<RecipeData>), 200)]
	public ActionResult<IEnumerable<RecipeData>> GetTrending()
	{
		var recipes = LoadData<List<RecipeData>>(_recipesFilePath);
		var trending = recipes.Take(10).ToImmutableList();
		return Ok(trending);
	}

	/// <summary>
	/// Retrieves popular recipes.
	/// </summary>
	/// <returns>A list of popular recipes.</returns>
	[HttpGet("popular")]
	[Produces("application/json")]
	[ProducesResponseType(typeof(IEnumerable<RecipeData>), 200)]
	public ActionResult<IEnumerable<RecipeData>> GetPopular()
	{
		var recipes = LoadData<List<RecipeData>>(_recipesFilePath);
		var popular = recipes.Take(15).ToImmutableList();
		return Ok(popular);
	}

	/// <summary>
	/// Retrieves favorited recipes for a specific user.
	/// </summary>
	/// <param name="userId">The user ID.</param>
	/// <returns>A list of favorited recipes.</returns>
	[HttpGet("favorited")]
	[Produces("application/json")]
	[ProducesResponseType(typeof(IEnumerable<RecipeData>), 200)]
	public ActionResult<IEnumerable<RecipeData>> GetFavorited([FromQuery] Guid userId)
	{
		var savedRecipes = LoadData<List<Guid>>(_savedRecipesFilePath);

		var recipes = LoadData<List<RecipeData>>(_recipesFilePath);
		var favorited = recipes
			.Where(r => savedRecipes.Contains(r.Id))
			.Select(r =>
			{
				r.IsFavorite = true;
				return r;
			})
			.ToImmutableList();

		return Ok(favorited);
	}

	/// <summary>
	/// Adds or removes a recipe from the user's favorites.
	/// </summary>
	/// <param name="recipeId">The ID of the recipe.</param>
	/// <param name="userId">The user ID.</param>
	/// <returns>No content.</returns>
	[HttpPost("favorited")]
	public IActionResult ToggleFavorite([FromQuery] Guid recipeId, [FromQuery] Guid userId) =>
		// We do not persist the favorite state in this example.
		NoContent();

	/// <summary>
	/// Saves or unsaves a recipe for a specific user.
	/// </summary>
	/// <param name="recipe">The recipe data.</param>
	/// <param name="userId">The user ID.</param>
	/// <returns>No content.</returns>
	[HttpPost]
	public IActionResult Save([FromBody] RecipeData recipe, [FromQuery] Guid userId) =>
		// We do not persist the favorite state in this example.
		NoContent();

	/// <summary>
	/// Creates a review for a recipe.
	/// </summary>
	/// <param name="reviewData">The review data.</param>
	/// <param name="userId">The user ID.</param>
	/// <returns>The created review.</returns>
	[HttpPost("review")]
	[Produces("application/json")]
	[ProducesResponseType(typeof(ReviewData), 201)]
	[ProducesResponseType(404)]
	public ActionResult<ReviewData> CreateReview([FromBody] ReviewData reviewData, [FromQuery] Guid userId)
	{
		var recipes = LoadData<List<RecipeData>>(_recipesFilePath);
		var recipe = recipes.FirstOrDefault(r => r.Id == reviewData.RecipeId);

		if (recipe != null)
		{
			reviewData.CreatedBy = userId;
			reviewData.Date = DateTime.Now;
			recipe.Reviews?.Add(reviewData);

			return Created("", reviewData);
		}
		else
		{
			return NotFound("Recipe not found");
		}
	}

	/// <summary>
	/// Likes a review for a recipe.
	/// </summary>
	/// <param name="reviewData">The review data.</param>
	/// <param name="userId">The user ID.</param>
	/// <returns>The updated review.</returns>
	[HttpPost("review/like")]
	[ProducesResponseType(typeof(ReviewData), 200)]
	[ProducesResponseType(404)]
	public ActionResult<ReviewData> LikeReview([FromBody] ReviewData reviewData, [FromQuery] Guid userId)

	{
		var recipes = LoadData<List<RecipeData>>(_recipesFilePath);
		var review = recipes.SelectMany(r => r.Reviews)
			.FirstOrDefault(x => x.Id == reviewData.Id && x.RecipeId == reviewData.RecipeId);

		if (review != null)
		{
			if (review.Dislikes != null && review.Dislikes.Contains(userId))
			{
				review.Dislikes.Remove(userId);
			}

			if (review.Likes == null)
			{
				review.Likes = new List<Guid>();
			}

			if (review.Likes.Contains(userId))
			{
				review.Likes.Remove(userId);
				review.UserLike = null;
			}
			else
			{
				review.Likes.Add(userId);
				review.UserLike = true;
			}

			return Ok(review);
		}
		return NotFound("Review not found");
	}

	/// <summary>
	/// Dislikes a review for a recipe.
	/// </summary>
	/// <param name="reviewData">The review data.</param>
	/// <param name="userId">The user ID.</param>
	/// <returns>The updated review.</returns>
	[HttpPost("review/dislike")]
	[Produces("application/json")]
	[ProducesResponseType(typeof(ReviewData), 200)]
	[ProducesResponseType(404)]
	public IActionResult DislikeReview([FromBody] ReviewData reviewData, [FromQuery] Guid userId)
	{
		var recipes = LoadData<List<RecipeData>>(_recipesFilePath);
		var review = recipes.SelectMany(r => r.Reviews)
			.FirstOrDefault(x => x.Id == reviewData.Id && x.RecipeId == reviewData.RecipeId);

		if (review != null)
		{
			if (review.Likes != null && review.Likes.Contains(userId))
			{
				review.Likes.Remove(userId);
			}

			if (review.Dislikes == null)
			{
				review.Dislikes = new List<Guid>();
			}

			if (review.Dislikes.Contains(userId))
			{
				review.Dislikes.Remove(userId);
				review.UserLike = null;
			}
			else
			{
				review.Dislikes.Add(userId);
				review.UserLike = false;
			}

			return Ok(review);
		}
		else
		{
			return NotFound("Review not found");
		}
	}

	/// <summary>
	/// Retrieves reviews for a specific recipe.
	/// </summary>
	/// <param name="recipeId">The recipe ID.</param>
	/// <returns>A list of reviews.</returns>
	[HttpGet("{recipeId}/reviews")]
	[Produces("application/json")]
	[ProducesResponseType(typeof(IEnumerable<ReviewData>), 200)]
	[ProducesResponseType(404)]
	public ActionResult<IEnumerable<ReviewData>> GetReviews(Guid recipeId)
	{
		var recipes = LoadData<List<RecipeData>>(_recipesFilePath);
		var recipe = recipes.FirstOrDefault(r => r.Id == recipeId);

		if (recipe != null)
		{
			return Ok(recipe.Reviews.ToImmutableList());
		}

		return NotFound("Recipe not found");
	}

	/// <summary>
	/// Retrieves steps for a specific recipe.
	/// </summary>
	/// <param name="recipeId">The recipe ID.</param>
	/// <returns>A list of steps.</returns>
	[HttpGet("{recipeId}/steps")]
	[Produces("application/json")]
	[ProducesResponseType(typeof(IEnumerable<StepData>), 200)]
	[ProducesResponseType(404)]
	public ActionResult<IEnumerable<StepData>> GetSteps(Guid recipeId)
	{
		var recipes = LoadData<List<RecipeData>>(_recipesFilePath);
		var recipe = recipes.FirstOrDefault(r => r.Id == recipeId);

		if (recipe != null)
		{
			return Ok(recipe.Steps.ToImmutableList());
		}
		else
		{
			return NotFound("Recipe not found");
		}
	}

	/// <summary>
	/// Retrieves ingredients for a specific recipe.
	/// </summary>
	/// <param name="recipeId">The recipe ID.</param>
	/// <returns>A list of ingredients.</returns>
	[HttpGet("{recipeId}/ingredients")]
	[Produces("application/json")]
	[ProducesResponseType(typeof(IEnumerable<IngredientData>), 200)]
	[ProducesResponseType(404)]
	public ActionResult<IEnumerable<IngredientData>> GetIngredients(Guid recipeId)
	{
		var recipes = LoadData<List<RecipeData>>(_recipesFilePath);
		var recipe = recipes.FirstOrDefault(r => r.Id == recipeId);

		if (recipe != null)
		{
			return Ok(recipe.Ingredients.ToImmutableList());
		}

		return NotFound("Recipe not found");
	}
}
