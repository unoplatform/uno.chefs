using System.Collections.Immutable;

namespace Chefs.Data;

public interface IRecipeEndpoint
{
    ValueTask<IImmutableList<RecipeData>> GetAll(CancellationToken ct);

    ValueTask<IImmutableList<RecipeData>> GetTrending(CancellationToken ct);

    ValueTask<IImmutableList<CategoryData>> GetCategories(CancellationToken ct);

    ValueTask<IImmutableList<UserData>> GetPopularCreators(CancellationToken ct);

    ValueTask<IImmutableList<CookbookData>> GetSavedCookbooks(CancellationToken ct);

    ValueTask AddCookbook(CookbookData cookbook, CancellationToken ct);
}
