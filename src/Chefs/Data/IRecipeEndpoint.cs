using System.Collections.Immutable;

namespace Chefs.Data;

public interface IRecipeEndpoint
{
    ValueTask<IImmutableList<RecipeData>> GetAll(CancellationToken ct);
    ValueTask<IImmutableList<CategoryData>> GetCategories(CancellationToken ct);
    ValueTask<IImmutableList<PopularCreatorData>> GetPopularCreators(CancellationToken ct);
    ValueTask<IImmutableList<CookbookData>> GetCookbooks(CancellationToken ct);
}
