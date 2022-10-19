using System.Collections.Immutable;
using Uno.Extensions.Serialization;
using Uno.Extensions.Storage;

namespace Chefs.Data;

public class RecipeEndpoint : IRecipeEndpoint
{
    public const string RecipeDataFile = "recipes.json";
    public const string CategoryDataFile = "categories.json";
    public const string CookbookDataFile = "cookbooks.json";
    public const string PopularCreatorDataFile = "popularcreators.json";

    private readonly IStorage _dataService;
    private readonly ISerializer _serializer;

    public RecipeEndpoint(IStorage dataService, ISerializer serializer)
    {
        _dataService = dataService;
        _serializer = serializer;
    }

    public async ValueTask<IImmutableList<RecipeData>> GetAll(CancellationToken ct)
    {
        var recipes = await _dataService.ReadFileAsync<IImmutableList<RecipeData>>(_serializer, RecipeDataFile);

        return recipes ?? ImmutableList<RecipeData>.Empty;
    }

    public async ValueTask<IImmutableList<CategoryData>> GetCategories(CancellationToken ct)
    {
        var categories = await _dataService.ReadFileAsync<IImmutableList<CategoryData>>(_serializer, CategoryDataFile);

        return categories ?? ImmutableList<CategoryData>.Empty;
    }

    public async ValueTask<IImmutableList<CookbookData>> GetCookbooks(CancellationToken ct)
    {
        var cookbooks = await _dataService.ReadFileAsync<IImmutableList<CookbookData>>(_serializer, CookbookDataFile);

        return cookbooks ?? ImmutableList<CookbookData>.Empty;
    }

    public async ValueTask<IImmutableList<PopularCreatorData>> GetPopularCreators(CancellationToken ct)
    {
        var popularCreators = await _dataService.ReadFileAsync<IImmutableList<PopularCreatorData>>(_serializer, PopularCreatorDataFile);

        return popularCreators ?? ImmutableList<PopularCreatorData>.Empty;
    }
}
