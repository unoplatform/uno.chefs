using System.Text.Json.Serialization;
using Chefs.Services.Clients.Models;

namespace Chefs.Data;

[JsonSerializable(typeof(List<CookbookData>))]
[JsonSerializable(typeof(List<SavedCookbooksData>))]
[JsonSerializable(typeof(CookbookData))]
[JsonSerializable(typeof(RecipeData))]
[JsonSerializable(typeof(List<NotificationData>))]
[JsonSerializable(typeof(List<RecipeData>))]
[JsonSerializable(typeof(List<CategoryData>))]
[JsonSerializable(typeof(List<SavedRecipesData>))]
[JsonSerializable(typeof(List<IngredientData>))]
[JsonSerializable(typeof(List<UserData>))]
[JsonSerializable(typeof(List<StepData>))]
[JsonSerializable(typeof(List<ReviewData>))]
[JsonSerializable(typeof(LoginRequest))]
[JsonSerializable(typeof(UserData))]
[JsonSerializable(typeof(Guid))]
[JsonSerializable(typeof(ReviewData))]
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase, PropertyNameCaseInsensitive = true)]
public partial class MockEndpointContext : JsonSerializerContext
{
}
