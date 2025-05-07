using System.Text.Json.Serialization;

namespace Chefs.Api.Serialization;

[JsonSerializable(typeof(ImmutableList<CookbookData>))]
[JsonSerializable(typeof(CookbookData))]
[JsonSerializable(typeof(RecipeData))]
[JsonSerializable(typeof(ImmutableList<NotificationData>))]
[JsonSerializable(typeof(ImmutableList<RecipeData>))]
[JsonSerializable(typeof(ImmutableList<CategoryData>))]
[JsonSerializable(typeof(ImmutableList<IngredientData>))]
[JsonSerializable(typeof(ImmutableList<UserData>))]
[JsonSerializable(typeof(ImmutableList<StepData>))]
[JsonSerializable(typeof(ImmutableList<ReviewData>))]
[JsonSerializable(typeof(UserData))]
[JsonSerializable(typeof(Guid))]
[JsonSerializable(typeof(ReviewData))]
[JsonSerializable(typeof(IEnumerable<RecipeData>))]
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase, PropertyNameCaseInsensitive = true)]
public partial class ChefsContext : JsonSerializerContext
{
}
