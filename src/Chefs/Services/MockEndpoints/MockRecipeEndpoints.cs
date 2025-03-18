using System.Text.Json;

namespace Chefs.Services;

public class MockRecipeEndpoints(string basePath, ISerializer serializer) : BaseMockEndpoint
{
	public string HandleRecipesRequest(HttpRequestMessage request)
	{
		var recipesData = LoadData("Recipes.json");
		var allRecipes = serializer.FromString<List<RecipeData>>(recipesData);
		
		//Get all categories
		if (request.RequestUri.AbsolutePath.Contains("/api/recipe/categories"))
		{
			return HandleCategoriesRequest();
		}
		
		//Get trending recipes
		if (request.RequestUri.AbsolutePath.Contains("/api/recipe/trending"))
		{
			return serializer.ToString(allRecipes.Take(10));
		}
		
		//Get popular recipes
		if (request.RequestUri.AbsolutePath.Contains("/api/recipe/popular"))
		{
			return serializer.ToString(allRecipes.Take(10));
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
			return serializer.ToString(allRecipes);
		}
		
		//Like a review
		if (request.RequestUri.AbsolutePath.Contains("/api/recipe/review/like"))
		{
			var userId = ExtractUserIdFromQuery(request.RequestUri.Query);
			var parsedUserId = Guid.TryParse(userId, out var validUserId) ? validUserId : Guid.NewGuid();
			var reviewData =
				serializer.FromString<ReviewData>(request.Content.ReadAsStringAsync().Result);
			return LikeReview(allRecipes, reviewData, parsedUserId);
		}
		
		//Dislike a review
		if (request.RequestUri.AbsolutePath.Contains("/api/recipe/review/dislike"))
		{
			var userId = ExtractUserIdFromQuery(request.RequestUri.Query);
			var parsedUserId = Guid.TryParse(userId, out var validUserId) ? validUserId : Guid.NewGuid();
			var reviewData =
				serializer.FromString<ReviewData>(request.Content.ReadAsStringAsync().Result);
			return DislikeReview(allRecipes, reviewData, parsedUserId);
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
				return serializer.ToString(recipe);
			}
		}
		
		return "{}";
	}
	
	private string HandleCategoriesRequest()
	{
		var categoriesData = LoadData("categories.json");
		var allCategories = serializer.FromString<List<CategoryData>>(categoriesData);
		return serializer.ToString(allCategories);
	}
	
	private string GetRecipeSteps(List<RecipeData> allRecipes, string recipeId)
	{
		recipeId = recipeId.TrimEnd('/');
		
		if (Guid.TryParse(recipeId, out var parsedId))
		{
			var recipe = allRecipes.FirstOrDefault(r => r.Id == parsedId);
			if (recipe != null && recipe.Steps != null)
			{
				return serializer.ToString(recipe.Steps);
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
				return serializer.ToString(recipe.Ingredients);
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
				return serializer.ToString(recipe.Reviews);
			}
		}
		
		return "[]";
	}
	
	private string GetFavoritedRecipes(List<RecipeData> allRecipes, HttpRequestMessage request)
	{
		var savedRecipesData = LoadData("SavedRecipes.json");
		var savedRecipes = serializer.FromString<List<SavedRecipesData>>(savedRecipesData);
		
		var queryParams = request.RequestUri.Query;
		var userId = ExtractUserIdFromQuery(queryParams);
		var userSavedRecipes = savedRecipes?.FirstOrDefault(sr => sr.UserId == Guid.Parse(userId))?.SavedRecipes;
		
		var favoritedRecipes = allRecipes?.Where(r => userSavedRecipes != null);
		return serializer.ToString(favoritedRecipes);
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
			
			return serializer.ToString(review);
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
			
			return serializer.ToString(review);
		}
		
		return "{}";
	}
	
	private string? ExtractUserIdFromQuery(string queryParams)
	{
		var queryDictionary = System.Web.HttpUtility.ParseQueryString(queryParams);
		return queryDictionary["userId"];
	}
}
