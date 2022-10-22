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
    public const string TrendingDataFile = "trendingrecipes.json";

    private readonly IStorage _dataService;
    private readonly ISerializer _serializer;

    public RecipeEndpoint(IStorage dataService, ISerializer serializer)
        => (_dataService, _serializer) = (dataService, serializer);

    public async ValueTask<IImmutableList<RecipeData>> GetAll(int userId, CancellationToken ct) => await _dataService
        .ReadFileAsync<IImmutableList<RecipeData>>(_serializer, RecipeDataFile) 
        ?? ImmutableList<RecipeData>.Empty;

    public async ValueTask<IImmutableList<CategoryData>> GetCategories(CancellationToken ct) => await _dataService
        .ReadFileAsync<IImmutableList<CategoryData>>(_serializer, CategoryDataFile)
        ?? ImmutableList<CategoryData>.Empty;

    public async ValueTask<IImmutableList<RecipeData>> GetTrending(int userId, CancellationToken ct) => await _dataService
        .ReadFileAsync<IImmutableList<RecipeData>>(_serializer, TrendingDataFile)
        ?? ImmutableList<RecipeData>.Empty;
}
