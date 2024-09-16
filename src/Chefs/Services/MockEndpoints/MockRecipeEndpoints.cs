using System.Text.Json;

namespace Chefs.Services;

public class MockRecipeEndpoints(string basePath, JsonSerializerOptions serializerOptions)
{
	public string HandleRecipesRequest(HttpRequestMessage request)
	
	{
		var recipesData = File.ReadAllText(Path.Combine(basePath, "Recipes.json"));
		var allRecipes = JsonSerializer.Deserialize<List<RecipeData>>(recipesData, serializerOptions);
		
		//Get all categories
		if (request.RequestUri.AbsolutePath.Contains("/api/recipe/categories"))
		{
			return HandleCategoriesRequest();
		}
		
		//Get trending recipes
		if (request.RequestUri.AbsolutePath.Contains("/api/recipe/trending"))
		{
			return JsonSerializer.Serialize(allRecipes.Take(10), serializerOptions);
		}
		
		//Get popular recipes
		if (request.RequestUri.AbsolutePath.Contains("/api/recipe/popular"))
		{
			return JsonSerializer.Serialize(allRecipes.Take(10), serializerOptions);
		}
		
		//Get favorited recipes
		if (request.RequestUri.AbsolutePath.Contains("/api/recipe/favorited"))
		{
			return GetFavoritedRecipes(allRecipes, request);
		}
		
		//Get recipe steps
		if (request.RequestUri.AbsolutePath.Contains("/steps"))
		{
			var recipeId = request.RequestUri.Segments[^2];
			return GetRecipeSteps(allRecipes, recipeId);
		}
		
		//Get recipe ingredients
		if (request.RequestUri.AbsolutePath.Contains("/ingredients"))
		{
			var recipeId = request.RequestUri.Segments[^2];
			return GetRecipeIngredients(allRecipes, recipeId);
		}
		
		//Get recipe reviews
		if (request.RequestUri.AbsolutePath.Contains("/reviews"))
		{
			var recipeId = request.RequestUri.Segments[^2];
			return GetRecipeReviews(allRecipes, recipeId);
		}
		
		//Get all recipes
		if (request.RequestUri.AbsolutePath == "/api/recipe")
		{
			return JsonSerializer.Serialize(allRecipes, serializerOptions);
		}
		
		//Like a review
		if (request.RequestUri.AbsolutePath.Contains("/api/recipe/review/like"))
		{
			var userId = ExtractUserIdFromQuery(request.RequestUri.Query);
			var reviewData =
				JsonSerializer.Deserialize<ReviewData>(request.Content.ReadAsStringAsync().Result, serializerOptions);
			return LikeReview(allRecipes, reviewData, Guid.Parse(userId));
		}
		
		//Dislike a review
		if (request.RequestUri.AbsolutePath.Contains("/api/recipe/review/dislike"))
		{
			var userId = ExtractUserIdFromQuery(request.RequestUri.Query);
			var reviewData =
				JsonSerializer.Deserialize<ReviewData>(request.Content.ReadAsStringAsync().Result, serializerOptions);
			return DislikeReview(allRecipes, reviewData, Guid.Parse(userId));
		}
		
		var specificRecipeId = request.RequestUri.Segments.Last();
		return GetRecipeDetails(allRecipes, specificRecipeId);
	}
	
	private string GetRecipeDetails(List<RecipeData> allRecipes, string recipeId)
	{
		recipeId = recipeId.TrimEnd('/');
		
		if (!string.IsNullOrEmpty(recipeId) && Guid.TryParse(recipeId, out var parsedId))
		{
			var recipe = allRecipes.FirstOrDefault(r => r.Id == parsedId);
			if (recipe != null)
			{
				return JsonSerializer.Serialize(recipe, serializerOptions);
			}
		}
		
		return "{}";
	}
	
	private string HandleCategoriesRequest()
	{
		var categoriesData = File.ReadAllText(Path.Combine(basePath, "categories.json"));
		var allCategories = JsonSerializer.Deserialize<List<CategoryData>>(categoriesData, serializerOptions);
		return JsonSerializer.Serialize(allCategories, serializerOptions);
	}
	
	private string GetRecipeSteps(List<RecipeData> allRecipes, string recipeId)
	{
		recipeId = recipeId.TrimEnd('/');
		
		if (Guid.TryParse(recipeId, out var parsedId))
		{
			var recipe = allRecipes.FirstOrDefault(r => r.Id == parsedId);
			if (recipe != null && recipe.Steps != null)
			{
				return JsonSerializer.Serialize(recipe.Steps, serializerOptions);
			}
		}
		
		return "[]";
	}
	
	private string GetRecipeIngredients(List<RecipeData> allRecipes, string recipeId)
	{
		recipeId = recipeId.TrimEnd('/');
		
		if (Guid.TryParse(recipeId, out var parsedId))
		{
			var recipe = allRecipes.FirstOrDefault(r => r.Id == parsedId);
			if (recipe != null && recipe.Ingredients != null)
			{
				return JsonSerializer.Serialize(recipe.Ingredients, serializerOptions);
			}
		}
		
		return "[]";
	}
	
	private string GetRecipeReviews(List<RecipeData> allRecipes, string recipeId)
	{
		recipeId = recipeId.TrimEnd('/');
		
		if (Guid.TryParse(recipeId, out var parsedId))
		{
			var recipe = allRecipes.FirstOrDefault(r => r.Id == parsedId);
			if (recipe != null && recipe.Reviews != null)
			{
				return JsonSerializer.Serialize(recipe.Reviews, serializerOptions);
			}
		}
		
		return "[]";
	}
	
	private string GetFavoritedRecipes(List<RecipeData> allRecipes, HttpRequestMessage request)
	{
		var savedRecipesData = File.ReadAllText(Path.Combine(basePath, "SavedRecipes.json"));
		var savedRecipes = JsonSerializer.Deserialize<List<SavedRecipesData>>(savedRecipesData, serializerOptions);
		
		var queryParams = request.RequestUri.Query;
		var userId = ExtractUserIdFromQuery(queryParams);
		var userSavedRecipes = savedRecipes?.FirstOrDefault(sr => sr.UserId == Guid.Parse(userId))?.SavedRecipes;
		
		var favoritedRecipes = allRecipes?.Where(r => userSavedRecipes != null);
		return JsonSerializer.Serialize(favoritedRecipes, serializerOptions);
	}
	
	private string LikeReview(List<RecipeData> allRecipes, ReviewData reviewData, Guid userId)
	{
		var recipe = allRecipes.FirstOrDefault(r => r.Id == reviewData.RecipeId);
		var review = recipe?.Reviews?.FirstOrDefault(r => r.Id == reviewData.Id);
		
		if (review != null)
		{
			review.Dislikes?.Remove(userId);
			
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
			
			return JsonSerializer.Serialize(review, serializerOptions);
		}
		
		return "{}";
	}
	
	private string DislikeReview(List<RecipeData> allRecipes, ReviewData reviewData, Guid userId)
	{
		var recipe = allRecipes.FirstOrDefault(r => r.Id == reviewData.RecipeId);
		var review = recipe?.Reviews?.FirstOrDefault(r => r.Id == reviewData.Id);
		
		if (review != null)
		{
			review.Likes?.Remove(userId);
			
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
			
			return JsonSerializer.Serialize(review, serializerOptions);
		}
		
		return "{}";
	}
	
	private string? ExtractUserIdFromQuery(string queryParams)
	{
		var queryDictionary = System.Web.HttpUtility.ParseQueryString(queryParams);
		return queryDictionary["userId"];
	}
}
