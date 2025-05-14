namespace Chefs.Services;

public class MockRecipeEndpoints(string basePath, ISerializer serializer) : BaseMockEndpoint(serializer)
{
	public string HandleRecipesRequest(HttpRequestMessage request)
	{
		var savedList = LoadData<List<Guid>>("SavedRecipes.json") ?? [];

		var allRecipes = LoadData<List<RecipeData>>("Recipes.json") ?? [];

		allRecipes.ForEach((_, r) => r.IsFavorite = savedList.Contains(r.Id ?? Guid.Empty));

		var path = request.RequestUri.AbsolutePath;
		if (path.Contains("/api/Recipe/categories"))
		{
			return HandleCategoriesRequest();
		}

		if (path.Contains("/api/Recipe/trending"))
		{
			return serializer.ToString(allRecipes.Take(10));
		}

		if (path.Contains("/api/Recipe/popular"))
		{
			return serializer.ToString(allRecipes.Take(10));
		}

		if (path.Contains("/api/Recipe/favorited"))
		{
			return serializer.ToString(allRecipes.Where(r => r.IsFavorite ?? false).ToList());
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

		if (request.Method == HttpMethod.Get && path == "/api/Recipe")
		{
			return serializer.ToString(allRecipes);
		}

		if (path.Contains("/api/Recipe/review/like"))
		{
			var userId = ExtractUserIdFromQuery(request.RequestUri.Query);
			var parsedUserId = Guid.TryParse(userId, out var validUserId) ? validUserId : Guid.NewGuid();
			var reviewData = serializer.FromString<ReviewData>(request.Content.ReadAsStringAsync().Result);
			return LikeReview(allRecipes, reviewData, parsedUserId);
		}

		if (path.Contains("/api/Recipe/review/dislike"))
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
			if (recipe != null)
			{
				return serializer.ToString(recipe);
			}
		}

		return "{}";
	}

	private string HandleCategoriesRequest()
	{
		var allCategories = LoadData<List<CategoryData>>("categories.json")
							?? new List<CategoryData>();
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
				review.Likes = [];
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
				review.Dislikes = [];
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
