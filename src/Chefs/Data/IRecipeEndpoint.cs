using System.Collections.Immutable;

namespace Chefs.Data;

public interface IRecipeEndpoint
{
    ValueTask<IImmutableList<RecipeData>> GetAll(CancellationToken ct);

    ValueTask<IImmutableList<RecipeData>> GetTrending(CancellationToken ct);

    ValueTask<IImmutableList<RecipeData>> GetSaved(CancellationToken ct);

    ValueTask Save(RecipeData recipe, CancellationToken ct);

    ValueTask<ReviewData> CreateReview(ReviewData reviewData, CancellationToken ct);

    ValueTask<IImmutableList<CategoryData>> GetCategories(CancellationToken ct);
}
