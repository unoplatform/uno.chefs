using Chefs.Services;
using Chefs.Services.Clients;
using Chefs.Services.Settings;
using Chefs.Services.Sharing;
using Chefs.Views.Flyouts;
using Microsoft.Extensions.Configuration;
using Uno.Extensions.Http.Kiota;

namespace Chefs;

public partial class App : Application
{
	private void ConfigureAppBuilder(IApplicationBuilder builder)
	{
		builder
			// Add navigation support for toolkit controls such as TabBar and NavigationView
			.UseToolkitNavigation()
			.Configure(host => host
				.UseAuthentication(auth =>
					auth.AddCustom(custom =>
					{
						custom
							.Login(async (sp, dispatcher, credentials, cancellationToken) => await ProcessCredentials(credentials));
					}, name: "CustomAuth")
				)
				.UseHttp((context, services) =>
				{
					services.AddTransient<MockHttpMessageHandler>();
					services.AddKiotaClient<ChefsApiClient>(
						context,
						options: new EndpointOptions { Url = "http://localhost:5116" }
#if USE_MOCKS
						, configure: (builder, endpoint) => builder.ConfigurePrimaryAndInnerHttpMessageHandler<MockHttpMessageHandler>()
#endif
					);
				})
#if DEBUG
				// Switch to Development environment when running in DEBUG
				.UseEnvironment(Environments.Development)
#endif
				// Temporary until uno loggig is fixed in extensions.
				//.UseLogging(configure: (context, logBuilder) =>
				//{
				//	// Configure log levels for different categories of logging
				//	logBuilder.SetMinimumLevel(
				//		context.HostingEnvironment.IsDevelopment() ? LogLevel.Information : LogLevel.Warning);
				//}, enableUnoLogging: true)
				.UseConfiguration(configure: configBuilder =>
					configBuilder
						.EmbeddedSource<App>()
						.Section<AppConfig>()
						.Section<Credentials>()
						.Section<SearchHistory>()
				)

				// Enable localization (see appsettings.json for supported languages)
				.UseLocalization()

				// Register Json serializers (ISerializer)
				.UseSerialization(configure: ConfigureSerialization)
				.ConfigureServices((context, services) =>
				{
					services
						.AddSingleton<ICookbookService, CookbookService>()
						.AddSingleton<IMessenger, WeakReferenceMessenger>()
						.AddSingleton<INotificationService, NotificationService>()
						.AddSingleton<IRecipeService, RecipeService>()
						.AddSingleton<IShareService, ShareService>()
						.AddSingleton<ISettingsService, SettingsService>()
						.AddSingleton<IUserService, UserService>();
				})
				.ConfigureAppConfiguration(config =>
				{
					// Clear any launchurl to make sure we always start at beginning
					// Deeplinking issue https://github.com/unoplatform/uno.chefs/issues/738
					var appsettingsPrefix = new Dictionary<string, string?>
							{
								{ HostingConstants.LaunchUrlKey, "" }
							};
					config.AddInMemoryCollection(appsettingsPrefix);
				})
				.UseNavigation(ReactiveViewModelMappings.ViewModelMappings, RegisterRoutes,
					configure: navConfig => navConfig with { AddressBarUpdateEnabled = false },
					configureServices: ConfigureNavServices));
	}

	private async ValueTask<IDictionary<string, string>?> ProcessCredentials(IDictionary<string, string> credentials)
	{
		// Check for username to simulate credential processing
		if (!(credentials?.TryGetValue("Username", out var username) ??
				false && !string.IsNullOrEmpty(username)))
		{
			return null;
		}

		// Simulate successful authentication by creating a dummy token dictionary
		var tokenDictionary = new Dictionary<string, string>
		{
			{ TokenCacheExtensions.AccessTokenKey, "SampleToken" },
			{ TokenCacheExtensions.RefreshTokenKey, "RefreshToken" },
			{ "Expiry", DateTime.Now.AddMinutes(5).ToString("g") } // Set token expiry
		};

		return tokenDictionary;
	}

	private void ConfigureSerialization(HostBuilderContext context, IServiceCollection services)
	{
#if USE_MOCKS
		services
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
			.AddJsonTypeInfo(MockEndpointContext.Default.IEnumerableSavedRecipesData);
#endif

		services
			.AddJsonTypeInfo(AppConfigContext.Default.AppConfig)
			.AddJsonTypeInfo(AppConfigContext.Default.DictionaryStringAppConfig)
			.AddJsonTypeInfo(AppConfigContext.Default.String);
	}

	private void ConfigureNavServices(HostBuilderContext context, IServiceCollection services)
	{
		services.AddTransient<Flyout, ResponsiveDrawerFlyout>();
	}

	private static void RegisterRoutes(IViewRegistry views, IRouteRegistry routes)
	{
		views.Register(
			new ViewMap(ViewModel: typeof(ShellModel)),
			new ViewMap<MainPage, MainModel>(),
			new ViewMap<WelcomePage, WelcomeModel>(),
			new DataViewMap<FiltersPage, FilterModel, SearchFilter>(),
			new ViewMap<HomePage, HomeModel>(),
			new DataViewMap<CreateUpdateCookbookPage, CreateUpdateCookbookModel, Cookbook>(),
			new ViewMap<LoginPage, LoginModel>(ResultData: typeof(Credentials)),
			new ViewMap<RegistrationPage, RegistrationModel>(),
			new ViewMap<NotificationsPage, NotificationsModel>(),
			new ViewMap<ProfilePage, ProfileModel>(Data: new DataMap<User>(), ResultData: typeof(IChefEntity)),
			new ViewMap<RecipeDetailsPage, RecipeDetailsModel>(Data: new DataMap<Recipe>()),
			new ViewMap<FavoriteRecipesPage, FavoriteRecipesModel>(),
			new DataViewMap<SearchPage, SearchModel, SearchFilter>(),
			new ViewMap<SettingsPage, SettingsModel>(Data: new DataMap<User>()),
			new ViewMap<LiveCookingPage, LiveCookingModel>(Data: new DataMap<LiveCookingParameter>()),
			new ViewMap<CookbookDetailPage, CookbookDetailModel>(Data: new DataMap<Cookbook>()),
			new ViewMap<CompletedDialog>(),
			new ViewMap<MapPage, MapModel>(),
			new ViewMap<GenericDialog, GenericDialogModel>(Data: new DataMap<DialogInfo>())
		);

		routes.Register(
			new RouteMap("", View: views.FindByViewModel<ShellModel>(),
				Nested: new RouteMap[]
				{
					new RouteMap("Welcome", View: views.FindByViewModel<WelcomeModel>()),
					new RouteMap("Login", View: views.FindByViewModel<LoginModel>()),
					new RouteMap("Register", View: views.FindByViewModel<RegistrationModel>()),
					new RouteMap("Main", View: views.FindByViewModel<MainModel>(), Nested:
					[
						#region Main Tabs
						new RouteMap("Home", View: views.FindByViewModel<HomeModel>(), IsDefault: true),
						new RouteMap("Search", View: views.FindByViewModel<SearchModel>()),
						new RouteMap("FavoriteRecipes", View: views.FindByViewModel<FavoriteRecipesModel>()),
						#endregion

						new RouteMap("CookbookDetails", View: views.FindByViewModel<CookbookDetailModel>()),
						new RouteMap("UpdateCookbook", View: views.FindByViewModel<CreateUpdateCookbookModel>()),
						new RouteMap("CreateCookbook", View: views.FindByViewModel<CreateUpdateCookbookModel>()),

						new RouteMap("RecipeInfo", View: views.FindByViewModel<RecipeDetailsModel>()),
						new RouteMap("LiveCooking", View: views.FindByViewModel<LiveCookingModel>()),
#if !IS_WASM_SKIA
						new RouteMap("Map", View: views.FindByViewModel<MapModel>()),
#endif
					]),
					new RouteMap("Notifications", View: views.FindByViewModel<NotificationsModel>()),
					new RouteMap("Filter", View: views.FindByViewModel<FilterModel>()),
					new RouteMap("Profile", View: views.FindByViewModel<ProfileModel>()),
					new RouteMap("Settings", View: views.FindByViewModel<SettingsModel>()),
					new RouteMap("Completed", View: views.FindByView<CompletedDialog>()),
					new RouteMap("Map", View: views.FindByViewModel<MapModel>()),
					new RouteMap("Dialog", View: views.FindByView<GenericDialog>())
				}
			)
		);
	}
}
