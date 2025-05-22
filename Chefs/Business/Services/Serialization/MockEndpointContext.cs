using System.Text.Json.Serialization;

namespace Chefs.Data;

[JsonSerializable(typeof(List<CookbookData>))]
[JsonSerializable(typeof(CookbookData))]
[JsonSerializable(typeof(RecipeData))]
[JsonSerializable(typeof(List<NotificationData>))]
[JsonSerializable(typeof(List<RecipeData>))]
[JsonSerializable(typeof(List<CategoryData>))]
[JsonSerializable(typeof(List<IngredientData>))]
[JsonSerializable(typeof(List<UserData>))]
[JsonSerializable(typeof(List<StepData>))]
[JsonSerializable(typeof(List<ReviewData>))]
[JsonSerializable(typeof(UserData))]
[JsonSerializable(typeof(Guid))]
[JsonSerializable(typeof(ReviewData))]
[JsonSerializable(typeof(IEnumerable<RecipeData>))]
[JsonSerializable(typeof(TimeSpanObject))]
[JsonSerializable(typeof(LoginRequest))]
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase, PropertyNameCaseInsensitive = true)]
public partial class MockEndpointContext : JsonSerializerContext
{
}
