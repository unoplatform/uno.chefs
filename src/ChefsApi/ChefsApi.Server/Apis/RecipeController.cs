using System.Collections.Immutable;
using System.Text.Json;
using Chefs.Data;

namespace ChefsApi.Server.Apis;

/// <summary>
/// Recipe Endpoints
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class RecipeController : ControllerBase
{
    private readonly string _recipesFilePath = "Data/AppData/Recipes.json";
    private readonly string _savedRecipesFilePath = "Data/AppData/SavedRecipes.json";
    private readonly string _categoriesFilePath = "Data/AppData/Categories.json";

    /// <summary>
    /// Retrieves all recipes.
    /// </summary>
    /// <returns>A list of recipes.</returns>
    [HttpGet]
    public IActionResult GetAll()
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
    public IActionResult GetCount([FromQuery] Guid userId)
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
    public IActionResult GetCategories()
    {
        var categories = LoadData<List<CategoryData>>(_categoriesFilePath);
        return Ok(categories.ToImmutableList());
    }

    /// <summary>
    /// Retrieves trending recipes.
    /// </summary>
    /// <returns>A list of trending recipes.</returns>
    [HttpGet("trending")]
    public IActionResult GetTrending()
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
    public IActionResult GetPopular()
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
    public IActionResult GetFavorited([FromQuery] Guid userId)
    {
        var savedRecipes = LoadData<List<SavedRecipesData>>(_savedRecipesFilePath);
        var userSavedRecipes = savedRecipes.FirstOrDefault(sr => sr.UserId == userId)?.SavedRecipes ?? new Guid[0];

        var recipes = LoadData<List<RecipeData>>(_recipesFilePath);
        var favorited = recipes
            .Where(r => userSavedRecipes.Contains(r.Id))
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
    public IActionResult ToggleFavorite([FromQuery] Guid recipeId, [FromQuery] Guid userId)
    {
        var savedRecipes = LoadData<List<SavedRecipesData>>(_savedRecipesFilePath);
        var userSavedRecipe = savedRecipes.FirstOrDefault(sr => sr.UserId == userId);
        
        if (userSavedRecipe != null)
        {
            if (userSavedRecipe.SavedRecipes.Contains(recipeId))
            {
                userSavedRecipe.SavedRecipes = userSavedRecipe.SavedRecipes.Where(id => id != recipeId).ToArray();
            }
            else
            {
                userSavedRecipe.SavedRecipes = userSavedRecipe.SavedRecipes.Concat(new[] { recipeId }).ToArray();
            }
        }
        else
        {
            savedRecipes.Add(new SavedRecipesData { UserId = userId, SavedRecipes = new[] { recipeId } });
        }
        
        System.IO.File.WriteAllText(_savedRecipesFilePath, JsonSerializer.Serialize(savedRecipes));
        
        return NoContent();
    }
    
    /// <summary>
    /// Saves or unsaves a recipe for a specific user.
    /// </summary>
    /// <param name="recipe">The recipe data.</param>
    /// <param name="userId">The user ID.</param>
    /// <returns>No content.</returns>
    [HttpPost]
    public IActionResult Save([FromBody] RecipeData recipe, [FromQuery] Guid userId)
    {
        var savedRecipes = LoadData<List<SavedRecipesData>>(_savedRecipesFilePath);
        var userSavedRecipe = savedRecipes.FirstOrDefault(sr => sr.UserId == userId);

        if (userSavedRecipe != null)
        {
            if (userSavedRecipe.SavedRecipes.Contains(recipe.Id))
            {
                userSavedRecipe.SavedRecipes = userSavedRecipe.SavedRecipes.Where(id => id != recipe.Id).ToArray();
            }
            else
            {
                userSavedRecipe.SavedRecipes = userSavedRecipe.SavedRecipes.Concat(new[] { recipe.Id }).ToArray();
            }
        }
        else
        {
            savedRecipes.Add(new SavedRecipesData { UserId = userId, SavedRecipes = new[] { recipe.Id } });
        }

        System.IO.File.WriteAllText(_savedRecipesFilePath, JsonSerializer.Serialize(savedRecipes));

        return NoContent();
    }

    /// <summary>
    /// Creates a review for a recipe.
    /// </summary>
    /// <param name="reviewData">The review data.</param>
    /// <param name="userId">The user ID.</param>
    /// <returns>The created review.</returns>
    [HttpPost("review")]
    public IActionResult CreateReview([FromBody] ReviewData reviewData, [FromQuery] Guid userId)
    {
        var recipes = LoadData<List<RecipeData>>(_recipesFilePath);
        var recipe = recipes.FirstOrDefault(r => r.Id == reviewData.RecipeId);

        if (recipe != null)
        {
            reviewData.CreatedBy = userId;
            reviewData.Date = DateTime.Now;
            recipe.Reviews?.Add(reviewData);

            System.IO.File.WriteAllText(_recipesFilePath, JsonSerializer.Serialize(recipes));

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
    public IActionResult LikeReview([FromBody] ReviewData reviewData, [FromQuery] Guid userId)
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

            System.IO.File.WriteAllText(_recipesFilePath, JsonSerializer.Serialize(recipes));

            return Ok(review);
        }
        else
        {
            return NotFound("Review not found");
        }
    }

    /// <summary>
    /// Dislikes a review for a recipe.
    /// </summary>
    /// <param name="reviewData">The review data.</param>
    /// <param name="userId">The user ID.</param>
    /// <returns>The updated review.</returns>
    [HttpPost("review/dislike")]
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

            System.IO.File.WriteAllText(_recipesFilePath, JsonSerializer.Serialize(recipes));

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
    public IActionResult GetReviews(Guid recipeId)
    {
        var recipes = LoadData<List<RecipeData>>(_recipesFilePath);
        var recipe = recipes.FirstOrDefault(r => r.Id == recipeId);
        
        if (recipe != null)
        {
            return Ok(recipe.Reviews.ToImmutableList());
        }
        else
        {
            return NotFound("Recipe not found");
        }
    }
    
    /// <summary>
    /// Retrieves steps for a specific recipe.
    /// </summary>
    /// <param name="recipeId">The recipe ID.</param>
    /// <returns>A list of steps.</returns>
    [HttpGet("{recipeId}/steps")]
    public IActionResult GetSteps(Guid recipeId)
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
    public IActionResult GetIngredients(Guid recipeId)
    {
        var recipes = LoadData<List<RecipeData>>(_recipesFilePath);
        var recipe = recipes.FirstOrDefault(r => r.Id == recipeId);
        
        if (recipe != null)
        {
            return Ok(recipe.Ingredients.ToImmutableList());
        }
        else
        {
            return NotFound("Recipe not found");
        }
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
