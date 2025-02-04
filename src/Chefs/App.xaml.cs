using System.Text.Json;
using Chefs.Services;
using Chefs.Services.Clients;
using Chefs.Views.Flyouts;
using LiveChartsCore;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using Uno.Extensions.Http.Kiota;

namespace Chefs;

public partial class App : Application
{
	/// <summary>
	/// Initializes the singleton application object. This is the first line of authored code
	/// executed, and as such is the logical equivalent of main() or WinMain().
	/// </summary>
	public App()
	{
		this.InitializeComponent();
	}

	public static Window? MainWindow;
	public static IHost? Host { get; private set; }

	protected override async void OnLaunched(LaunchActivatedEventArgs args)
	{
		var builder = this.CreateBuilder(args)
			// Add navigation support for toolkit controls such as TabBar and NavigationView
			.UseToolkitNavigation()
			.Configure(host => host
				.UseAuthentication(auth =>
					auth.AddCustom(custom =>
					{
						custom
							.Login((sp, dispatcher, credentials, cancellationToken) =>
							{
								// Check for username to simulate credential processing
								if (!(credentials?.TryGetValue("Username", out var username) ??
									  false && !string.IsNullOrEmpty(username)))

								{
									return ValueTask.FromResult<IDictionary<string, string>?>(null);
								}

								// Simulate successful authentication by creating a dummy token dictionary
								var tokenDictionary = new Dictionary<string, string>
								{
									{ TokenCacheExtensions.AccessTokenKey, "SampleToken" },
									{ TokenCacheExtensions.RefreshTokenKey, "RefreshToken" },
									{ "Expiry", DateTime.Now.AddMinutes(5).ToString("g") } // Set token expiry
                                };
								return ValueTask.FromResult<IDictionary<string, string>?>(tokenDictionary);


							});
					}, name: "CustomAuth")
				)
				.UseHttp((context, services) =>
				{
				})
#if DEBUG
				// Switch to Development environment when running in DEBUG
				.UseEnvironment(Environments.Development)
#endif
				.UseLogging(configure: (context, logBuilder) =>
				{
					// Configure log levels for different categories of logging
					logBuilder.SetMinimumLevel(
						context.HostingEnvironment.IsDevelopment() ? LogLevel.Information : LogLevel.Warning);
				}, enableUnoLogging: true)
				.UseConfiguration(configure: configBuilder =>
					configBuilder
						.EmbeddedSource<App>()
						.Section<AppConfig>()
						.Section<Credentials>()
						.Section<SearchHistory>()
				)

				// Enable localization (see appsettings.json for supported languages)
				.UseLocalization()

				// Register Json serializers (ISerializer and ISerializer)
				.UseSerialization()
				.ConfigureServices((context, services) =>
				{
					services.AddSingleton<IRequestAdapter, HttpClientRequestAdapter>(sp =>
					{
						var mockHttpMessageHandler = new MockHttpMessageHandler();
						
						var httpClient = new HttpClient(mockHttpMessageHandler)
						{
							BaseAddress = new Uri("https://localhost:5002")
						};
						var authenticationProvider = new AnonymousAuthenticationProvider();
						var parseNodeFactory = new Microsoft.Kiota.Serialization.Json.JsonParseNodeFactory();
						var serializationWriterFactory = new Microsoft.Kiota.Serialization.Json.JsonSerializationWriterFactory();
						
						return new HttpClientRequestAdapter(authenticationProvider, parseNodeFactory, serializationWriterFactory, httpClient);
					});

					services.AddSingleton<ChefsApiClient>(sp =>
					{
						var requestAdapter = sp.GetRequiredService<IRequestAdapter>();
						return new ChefsApiClient(requestAdapter);
					});

					//UNCOMMENT THE CODE BELOW TO USE THE REAL API (WITH THE SERVER PROJECT) INSTEAD OF THE MOCK API 
					
					// services.AddKiotaClient<ChefsApiClient>(
					// 	context,
					// 	options: new EndpointOptions { Url = "https://localhost:5002" }
					// );

					services
						.AddSingleton<INotificationService, NotificationService>()
						.AddSingleton<IRecipeService, RecipeService>()
						.AddSingleton<IUserService, UserService>()
						.AddSingleton<ICookbookService, CookbookService>()
						.AddSingleton<IMessenger, WeakReferenceMessenger>();
				})
				.UseNavigation(ReactiveViewModelMappings.ViewModelMappings, RegisterRoutes,
					configureServices: ConfigureNavServices));

		LiveCharts.Configure(config =>
			config
				.HasMap<NutritionChartItem>((nutritionChartItem, point) =>
				{
					// here we use the index as X, and the nutrition value as Y 
					return new(point, nutritionChartItem.Value);
				})
		);
		MainWindow = builder.Window;

		Host = await builder.NavigateAsync<ShellControl>();

		var config = Host.Services.GetRequiredService<IOptions<AppConfig>>();
		var userService = Host.Services.GetRequiredService<IUserService>();
		var themeService = MainWindow.GetThemeService();
		var appTheme = config.Value?.IsDark switch
		{
			true => AppTheme.Dark,
			false => AppTheme.Light,
			_ => AppTheme.System
		};

		await userService.UpdateSettings(CancellationToken.None, isDark: themeService.IsDark);
		await themeService.SetThemeAsync(appTheme);
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
			new ViewMap<FiltersPage, FilterModel>(Data: new DataMap<SearchFilter>()),
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
					new RouteMap("Main", View: views.FindByViewModel<MainModel>(), Nested: new RouteMap[]
					{
                        #region Main Tabs
                        new RouteMap("Home", View: views.FindByViewModel<HomeModel>(), IsDefault: true),
						new RouteMap("Search", View: views.FindByViewModel<SearchModel>()),
						new RouteMap("FavoriteRecipes", View: views.FindByViewModel<FavoriteRecipesModel>()),
                        #endregion

                        #region Cookbooks
                        new RouteMap("CookbookDetails", View: views.FindByViewModel<CookbookDetailModel>(), DependsOn: "FavoriteRecipes"),
						new RouteMap("UpdateCookbook", View: views.FindByViewModel<CreateUpdateCookbookModel>(), DependsOn: "FavoriteRecipes"),
						new RouteMap("CreateCookbook", View: views.FindByViewModel<CreateUpdateCookbookModel>(), DependsOn: "FavoriteRecipes"),
                        #endregion

                        #region Recipe Details
                        new RouteMap("RecipeDetails", View: views.FindByViewModel<RecipeDetailsModel>(), DependsOn: "Home"),
						new RouteMap("SearchRecipeDetails", View: views.FindByViewModel<RecipeDetailsModel>(), DependsOn: "Search"),
						new RouteMap("FavoriteRecipeDetails", View: views.FindByViewModel<RecipeDetailsModel>(), DependsOn: "FavoriteRecipes"),
						new RouteMap("CookbookRecipeDetails", View: views.FindByViewModel<RecipeDetailsModel>(), DependsOn: "FavoriteRecipes"),
                        #endregion

                        #region Live Cooking
                        new RouteMap("LiveCooking", View: views.FindByViewModel<LiveCookingModel>(), DependsOn: "RecipeDetails"),
						new RouteMap("SearchLiveCooking", View: views.FindByViewModel<LiveCookingModel>(), DependsOn: "SearchRecipeDetails"),
						new RouteMap("FavoriteLiveCooking", View: views.FindByViewModel<LiveCookingModel>(), DependsOn: "FavoriteRecipeDetails"),
						new RouteMap("CookbookLiveCooking", View: views.FindByViewModel<LiveCookingModel>(), DependsOn: "CookbookRecipeDetails"),
                        #endregion

                        new RouteMap("Map", View: views.FindByViewModel<MapModel>(), DependsOn: "Home"),
					}),
					new RouteMap("Notifications", View: views.FindByViewModel<NotificationsModel>()),
					new RouteMap("Filter", View: views.FindByViewModel<FilterModel>()),
					new RouteMap("Profile", View: views.FindByViewModel<ProfileModel>()),
					new RouteMap("Settings", View: views.FindByViewModel<SettingsModel>(), DependsOn: "Profile"),
					new RouteMap("Completed", View: views.FindByView<CompletedDialog>()),
					new RouteMap("Map", View: views.FindByViewModel<MapModel>(), DependsOn: "Main"),
					new RouteMap("Dialog", View: views.FindByView<GenericDialog>())
				}
			)
		);
	}
}
