using System.Collections.Immutable;

namespace Chefs.Business;

public class RecipeService : IRecipeService
{
    public ValueTask<IImmutableList<Recipe>> GetAll(CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public ValueTask<IImmutableList<Recipe>> GetByCategory(Category category, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public ValueTask<Category> GetCategories(CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public ValueTask<IImmutableList<Cookbook>> GetCookbooks(CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public ValueTask<IImmutableList<User>> GetPopularCreators(CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public ValueTask<IImmutableList<Recipe>> GetRecent(CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public ValueTask<IImmutableList<Recipe>> GetTrending(CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public ValueTask<IImmutableList<Recipe>> Search(SearchFilter search, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}
