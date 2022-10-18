using System.Collections.Immutable;

namespace Chefs.Business;

public interface IRecipeService
{
    ValueTask<IImmutableList<Recipe>> GetAll(CancellationToken ct);
    ValueTask<IImmutableList<Recipe>> GetByCategory(Category category, CancellationToken ct);
    ValueTask<Category> GetCategories(CancellationToken ct);
    ValueTask<IImmutableList<Recipe>> GetTrending(CancellationToken ct);
    ValueTask<IImmutableList<Recipe>> GetRecent(CancellationToken ct);
    ValueTask<IImmutableList<User>> GetPopularCreators(CancellationToken ct);
    ValueTask<IImmutableList<Cookbook>> GetCookbooks(CancellationToken ct);
    ValueTask<IImmutableList<Recipe>> Search(SearchFilter search, CancellationToken ct);
}
