using System.Collections.Immutable;

namespace Chefs.Data;

public interface IRecipeEndpoint
{
    ValueTask<IImmutableList<RecipeData>> GetAll(int userId, CancellationToken ct);

    ValueTask<IImmutableList<RecipeData>> GetTrending(int userId, CancellationToken ct);

    ValueTask<IImmutableList<CategoryData>> GetCategories(CancellationToken ct);
}
