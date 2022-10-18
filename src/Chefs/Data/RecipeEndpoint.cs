using System.Collections.Immutable;

namespace Chefs.Data;

public class RecipeEndpoint : IRecipeEndpoint
{
    public ValueTask<IImmutableList<RecipeData>> GetAll(CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public ValueTask<IImmutableList<CategoryData>> GetCategories(CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public ValueTask<IImmutableList<CookbookData>> GetCookbooks(CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public ValueTask<IImmutableList<PopularCreatorData>> GetPopularCreators(CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}
