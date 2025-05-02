using System.Web;

namespace Chefs.Services;

public class MockRecipeEndpoints(string basePath, ISerializer serializer) : BaseMockEndpoint
{
	private static Dictionary<Guid, List<Guid>>? _userFavorites;

	public string HandleRecipesRequest(HttpRequestMessage request)
	{
		if (_userFavorites == null)
		{
			_userFavorites = new Dictionary<Guid, List<Guid>>();
			var savedData = LoadData("SavedRecipes.json");
			var savedList = serializer.FromString<List<SavedRecipesData>>(savedData);
			foreach (var entry in savedList!)
			{
				_userFavorites[entry.UserId] = entry.SavedRecipes?.ToList() ?? [];
			}
		}

		var recipesJson = LoadData("Recipes.json");
		var allRecipes = serializer.FromString<List<RecipeData>>(recipesJson);

		var userIdParam = ExtractUserIdFromQuery(request.RequestUri.Query)
		                  ?? "3c896419-e280-40e7-8552-240635566fed";
		if (!Guid.TryParse(userIdParam, out var currentUserId))
		{
			currentUserId = Guid.Parse("3c896419-e280-40e7-8552-240635566fed");
		}

		if (!_userFavorites.ContainsKey(currentUserId))
		{
			_userFavorites[currentUserId] = [];
		}

		if (request.Method == HttpMethod.Post
		    && request.RequestUri.AbsolutePath.Contains("/api/recipe/favorited"))
		{
			var userId = _userFavorites[currentUserId];
			var queryParam = HttpUtility.ParseQueryString(request.RequestUri.Query);
			if (Guid.TryParse(queryParam["RecipeId"], out var recipeId))
			{
				if (userId.Contains(recipeId))
				{
					userId.Remove(recipeId);
				}
				else
				{
					userId.Add(recipeId);
				}
			}

			var updated = allRecipes
				.Where(r => userId.Contains(r.Id))
				.Select(r =>
				{
					r.IsFavorite = true;
					return r;
				})
				.ToList();
			return serializer.ToString(updated);
		}

		var favs = _userFavorites[currentUserId];
		allRecipes.ForEach(r => r.IsFavorite = favs.Contains(r.Id));

		var path = request.RequestUri.AbsolutePath;
		if (path.Contains("/api/recipe/categories"))
		{
			return HandleCategoriesRequest();
		}

		if (path.Contains("/api/recipe/trending"))
		{
			return serializer.ToString(allRecipes.Take(10));
		}

		if (path.Contains("/api/recipe/popular"))
		{
			return serializer.ToString(allRecipes.Take(10));
		}

		if (path.Contains("/api/recipe/favorited"))
		{
			return serializer.ToString(allRecipes.Where(r => r.IsFavorite).ToList());
		}

		if (path.Contains("/steps"))
		{
			return GetRecipeSteps(allRecipes, request.RequestUri.Segments[^2]);
		}

		if (path.Contains("/ingredients"))
		{
			return GetRecipeIngredients(allRecipes, request.RequestUri.Segments[^2]);
		}

		if (path.Contains("/reviews"))
		{
			return GetRecipeReviews(allRecipes, request.RequestUri.Segments[^2]);
		}

		if (request.Method == HttpMethod.Get && path == "/api/recipe")
		{
			return serializer.ToString(allRecipes);
		}

		if (path.Contains("/api/recipe/review/like"))
		{
			var userId = ExtractUserIdFromQuery(request.RequestUri.Query);
			var parsedUserId = Guid.TryParse(userId, out var validUserId) ? validUserId : Guid.NewGuid();
			var reviewData = serializer.FromString<ReviewData>(request.Content.ReadAsStringAsync().Result);
			return LikeReview(allRecipes, reviewData, parsedUserId);
		}

		if (path.Contains("/api/recipe/review/dislike"))
		{
			var userId = ExtractUserIdFromQuery(request.RequestUri.Query);
			var parsedUserId = Guid.TryParse(userId, out var validUserId) ? validUserId : Guid.NewGuid();
			var reviewData =
				serializer.FromString<ReviewData>(request.Content.ReadAsStringAsync().Result);
			return DislikeReview(allRecipes, reviewData, parsedUserId);
		}

		return GetRecipeDetails(allRecipes, request.RequestUri.Segments.Last());
	}

	private string GetRecipeDetails(List<RecipeData> allRecipes, string recipeId)
	{
		recipeId = recipeId.TrimEnd('/');
		if (Guid.TryParse(recipeId, out var gid))
		{
			var recipe = allRecipes.FirstOrDefault(x => x.Id == gid);
			if (recipe != null) return serializer.ToString(recipe);
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
