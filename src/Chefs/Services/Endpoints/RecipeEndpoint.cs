namespace Chefs.Services.Endpoints;

public class RecipeEndpoint : IRecipeEndpoint
{
	private readonly IStorage _dataService;
	private readonly ISerializer _serializer;
	private readonly IUserEndpoint _userEndpoint;

	private List<SavedRecipesData>? _savedRecipes;
	private List<RecipeData>? _recipes;
	private List<CategoryData>? _categories;

	public RecipeEndpoint(IStorage dataService, ISerializer serializer, IUserEndpoint userEndpoint)
		=> (_dataService, _serializer, _userEndpoint) = (dataService, serializer, userEndpoint);

	public async ValueTask<IImmutableList<RecipeData>> GetAll(CancellationToken ct) => (await Load()).ToImmutableList()
		?? ImmutableList<RecipeData>.Empty;

	public async ValueTask<int> GetCount(Guid userId, CancellationToken ct) => (await Load())
		.Where(x => x.UserId == userId)
		.Count();

	public async ValueTask<IImmutableList<CategoryData>> GetCategories(CancellationToken ct) => (await LoadCategories())
		.ToImmutableList()
		?? ImmutableList<CategoryData>.Empty;

	public async ValueTask<IImmutableList<RecipeData>> GetTrending(CancellationToken ct) => (await Load())?
		.Take(10)
		.ToImmutableList()
		?? ImmutableList<RecipeData>.Empty;

	public async ValueTask<IImmutableList<RecipeData>> GetPopular(CancellationToken ct) => (await Load())?
		.Take(15)
		.ToImmutableList()
		?? ImmutableList<RecipeData>.Empty;

	public async ValueTask<IImmutableList<RecipeData>> GetSaved(CancellationToken ct)
	{
		var currentUser = await _userEndpoint.GetCurrent(ct);

		var recipes = await Load();

		var savedRecipes = (await LoadSaved())?
			.Where(x => x.UserId == currentUser.Id).FirstOrDefault();

		if (savedRecipes is not null && savedRecipes.SavedRecipes is not null)
		{
			return recipes?.Where(x => savedRecipes.SavedRecipes.Any(y => y == x.Id)).ToImmutableList() ?? ImmutableList<RecipeData>.Empty;
		}

		return ImmutableList<RecipeData>.Empty;
	}

	public async ValueTask Save(RecipeData recipe, CancellationToken ct)
	{
		var currentUser = await _userEndpoint.GetCurrent(ct);

		var savedRecipes = await LoadSaved();

		var userSavedRecipe = savedRecipes?.Where(x => x.UserId == currentUser.Id).FirstOrDefault();

		if (userSavedRecipe is not null && userSavedRecipe.SavedRecipes is not null)
		{
			userSavedRecipe.SavedRecipes = !userSavedRecipe.SavedRecipes.Contains(recipe.Id) ?
				userSavedRecipe.SavedRecipes.Concat(recipe.Id).ToArray() :
				userSavedRecipe.SavedRecipes.Where(id => recipe.Id != id).ToArray();
		}
		else
		{
			savedRecipes?.Add(new SavedRecipesData { UserId = currentUser.Id, SavedRecipes = new Guid[] { recipe.Id } });
		}

		_savedRecipes = savedRecipes!;
	}

	public async ValueTask<ReviewData> CreateReview(ReviewData reviewData, CancellationToken ct)
	{
		var currentUser = await _userEndpoint.GetCurrent(ct);

		var recipes = await Load();

		var recipe = recipes.Where(r => r.Id == reviewData.RecipeId).FirstOrDefault();

		if (recipe is not null)
		{
			reviewData.PublisherName = currentUser.FullName;
			reviewData.UrlAuthorImage = currentUser.UrlProfileImage;
			reviewData.CreatedBy = currentUser.Id;
			reviewData.Date = DateTime.Now;
			recipe.Reviews?.Add(reviewData);

			return reviewData;
		}
		else
		{
			throw new Exception();
		}
	}

	public async ValueTask<ReviewData> DislikeReview(ReviewData reviewData, CancellationToken ct)
	{
		var userId = (await _userEndpoint.GetCurrent(ct)).Id;

		var review = (await Load()).FirstOrDefault(r => r.Id == reviewData.RecipeId)?
			.Reviews?.FirstOrDefault(x => x.Id == reviewData.Id);

		if (review is not null)
		{
			if (review.Likes is not null && review.Likes.Contains(userId))
			{
				review.Likes.Remove(userId);
			}

			if (review.Dislikes is not null)
			{
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
			}
			else
			{
				review.Dislikes = new List<Guid> { userId };
				review.UserLike = false;
			}

			return review;
		}
		else
		{
			throw new Exception();
		}
	}

	public async ValueTask<ReviewData> LikeReview(ReviewData reviewData, CancellationToken ct)
	{
		var userId = (await _userEndpoint.GetCurrent(ct)).Id;

		var review = (await Load()).FirstOrDefault(r => r.Id == reviewData.RecipeId)?
			.Reviews?.FirstOrDefault(x => x.Id == reviewData.Id);

		if (review is not null)
		{
			if (review.Dislikes is not null && review.Dislikes.Contains(userId))
			{
				review.Dislikes.Remove(userId);
			}

			if (review.Likes is not null)
			{
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
			}
			else
			{
				review.Likes = new List<Guid> { userId };
				review.UserLike = true;
			}

			return review;
		}
		else
		{
			throw new Exception();
		}
	}

	//Implementation to update saved recipes in memory 
	private async ValueTask<IList<RecipeData>> Load()
	{
		if (_recipes == null)
		{
			_recipes = (await _dataService
				.ReadPackageFileAsync<List<RecipeData>>(_serializer, Constants.RecipeDataFile));
			var saved = await GetSaved(CancellationToken.None);

			var currentUser = await _userEndpoint.GetCurrent(CancellationToken.None);
			if (_recipes is not null)
			{
				foreach (RecipeData r in _recipes)
				{
					if (r.Reviews is not null)
					{
						foreach (ReviewData rev in r.Reviews)
						{
							if (rev.Likes is not null && rev.Likes.Contains(currentUser.Id))
							{
								rev.UserLike = true;
								break;
							}

							if (rev.Dislikes is not null && rev.Dislikes.Contains(currentUser.Id))
							{
								rev.UserLike = false;
							}
						}
					}
				}
				_recipes?.ForEach(x => x.Save = saved.Contains(x));
			}
		}

		return _recipes ?? new List<RecipeData>();
	}

	//Implementation to update saved cookbooks and recipes in memory 
	private async ValueTask<List<SavedRecipesData>> LoadSaved()
	{
		if (_savedRecipes == null)
		{
			_savedRecipes = (await _dataService
				.ReadPackageFileAsync<List<SavedRecipesData>>(_serializer, Constants.SavedRecipesDataFile));
		}
		return _savedRecipes ?? new List<SavedRecipesData>();
	}

	//Implementation categories to keep in memory 
	private async ValueTask<List<CategoryData>> LoadCategories()
	{
		if (_categories == null)
		{
			_categories = (await _dataService
				.ReadPackageFileAsync<List<CategoryData>>(_serializer, Constants.CategoryDataFile));
		}
		return _categories ?? new List<CategoryData>();
	}
}
