using System.Text.Json.Serialization;
using Chefs.Services.Clients.Models;
using CategoryData = Chefs.DataContracts.Entities.CategoryData;
using CookbookData = Chefs.DataContracts.Entities.CookbookData;
using IngredientData = Chefs.DataContracts.Entities.IngredientData;
using NotificationData = Chefs.DataContracts.Entities.NotificationData;
using RecipeData = Chefs.DataContracts.Entities.RecipeData;
using ReviewData = Chefs.DataContracts.Entities.ReviewData;
using StepData = Chefs.DataContracts.Entities.StepData;
using UserData = Chefs.DataContracts.Entities.UserData;

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
[JsonSerializable(typeof(IEnumerable<RecipeData>))]
[JsonSerializable(typeof(IEnumerable<SavedRecipesData>))]
[JsonSerializable(typeof(SavedRecipesData))]
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase, PropertyNameCaseInsensitive = true)]
public partial class MockEndpointContext : JsonSerializerContext
{
}
