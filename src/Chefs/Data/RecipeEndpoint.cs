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
        => (_dataService, _serializer) = (dataService, serializer);

    public async ValueTask<IImmutableList<RecipeData>> GetAll(CancellationToken ct) => await _dataService
        .ReadFileAsync<IImmutableList<RecipeData>>(_serializer, RecipeDataFile) 
        ?? ImmutableList<RecipeData>.Empty;

    public async ValueTask<IImmutableList<CategoryData>> GetCategories(CancellationToken ct) => await _dataService
        .ReadFileAsync<IImmutableList<CategoryData>>(_serializer, CategoryDataFile)
        ?? ImmutableList<CategoryData>.Empty;

    public async ValueTask<IImmutableList<CookbookData>> GetCookbooks(CancellationToken ct) => await _dataService
        .ReadFileAsync<IImmutableList<CookbookData>>(_serializer, CookbookDataFile)
        ?? ImmutableList<CookbookData>.Empty;

    public async ValueTask<IImmutableList<PopularCreatorData>> GetPopularCreators(CancellationToken ct) => 
        await _dataService.ReadFileAsync<IImmutableList<PopularCreatorData>>(_serializer, PopularCreatorDataFile)
        ?? ImmutableList<PopularCreatorData>.Empty;
}
