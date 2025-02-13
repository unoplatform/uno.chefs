namespace Chefs.Services.Endpoints;

public interface IRecipeEndpoint
{
	ValueTask<IImmutableList<RecipeData>> GetAll(CancellationToken ct);

	ValueTask<int> GetCount(Guid userId, CancellationToken ct);

	ValueTask<IImmutableList<RecipeData>> GetTrending(CancellationToken ct);

	ValueTask<IImmutableList<RecipeData>> GetPopular(CancellationToken ct);

	ValueTask<IImmutableList<RecipeData>> GetFavorited(CancellationToken ct);

	ValueTask Save(RecipeData recipe, CancellationToken ct);

	ValueTask<ReviewData> CreateReview(ReviewData reviewData, CancellationToken ct);

	ValueTask<IImmutableList<CategoryData>> GetCategories(CancellationToken ct);

	ValueTask<ReviewData> LikeReview(ReviewData reviewData, CancellationToken ct);

	ValueTask<ReviewData> DislikeReview(ReviewData reviewData, CancellationToken ct);
}
