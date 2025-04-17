
namespace Chefs.Services;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddSerializationTypeInfo(this IServiceCollection services)
	{
		services
#if USE_MOCKS
			.AddJsonTypeInfo(MockEndpointContext.Default.ListCookbookData)
			.AddJsonTypeInfo(MockEndpointContext.Default.ListSavedCookbooksData)
			.AddJsonTypeInfo(MockEndpointContext.Default.CookbookData)
			.AddJsonTypeInfo(MockEndpointContext.Default.RecipeData)
			.AddJsonTypeInfo(MockEndpointContext.Default.ListNotificationData)
			.AddJsonTypeInfo(MockEndpointContext.Default.ListRecipeData)
			.AddJsonTypeInfo(MockEndpointContext.Default.ListCategoryData)
			.AddJsonTypeInfo(MockEndpointContext.Default.ListSavedRecipesData)
			.AddJsonTypeInfo(MockEndpointContext.Default.ListIngredientData)
			.AddJsonTypeInfo(MockEndpointContext.Default.ListUserData)
			.AddJsonTypeInfo(MockEndpointContext.Default.ListStepData)
			.AddJsonTypeInfo(MockEndpointContext.Default.ListReviewData)
			.AddJsonTypeInfo(MockEndpointContext.Default.LoginRequest)
			.AddJsonTypeInfo(MockEndpointContext.Default.UserData)
			.AddJsonTypeInfo(MockEndpointContext.Default.Guid)
			.AddJsonTypeInfo(MockEndpointContext.Default.ReviewData)
			.AddJsonTypeInfo(MockEndpointContext.Default.SavedCookbooksData)
			.AddJsonTypeInfo(MockEndpointContext.Default.SavedRecipesData)
			.AddJsonTypeInfo(MockEndpointContext.Default.IEnumerableRecipeData)
			.AddJsonTypeInfo(MockEndpointContext.Default.IEnumerableSavedRecipesData)
#endif
			.AddJsonTypeInfo(ModelSerializerContext.Default.AppConfig)
			.AddJsonTypeInfo(ModelSerializerContext.Default.User)
			.AddJsonTypeInfo(ModelSerializerContext.Default.Credentials)
			.AddJsonTypeInfo(ModelSerializerContext.Default.SearchHistory)
			.AddJsonTypeInfo(ModelSerializerContext.Default.DictionaryStringAppConfig)
			.AddJsonTypeInfo(ModelSerializerContext.Default.DictionaryStringUser)
			.AddJsonTypeInfo(ModelSerializerContext.Default.DictionaryStringCredentials)
			.AddJsonTypeInfo(ModelSerializerContext.Default.DictionaryStringSearchHistory);

		return services;
	}
}
