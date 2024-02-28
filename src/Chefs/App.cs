using Chefs.Views.Flyouts;
using LiveChartsCore;

namespace Chefs;

public class App : Application
{
	private static Window? _window;
	public static IHost? Host { get; private set; }

	protected async override void OnLaunched(LaunchActivatedEventArgs args)
	{
		var builder = this.CreateBuilder(args)

			// Add navigation support for toolkit controls such as TabBar and NavigationView
			.UseToolkitNavigation()
			.Configure(host => host
#if DEBUG
			// Switch to Development environment when running in DEBUG
			.UseEnvironment(Environments.Development)
#endif
			.UseLogging(configure: (context, logBuilder) =>
			{
				// Configure log levels for different categories of logging
				logBuilder.SetMinimumLevel(
					context.HostingEnvironment.IsDevelopment() ?
						LogLevel.Information :
						LogLevel.Warning);
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
				services
					.AddSingleton<INotificationService, NotificationService>()
					.AddSingleton<IRecipeService, RecipeService>()
					.AddSingleton<IUserService, UserService>()
					.AddSingleton<ICookbookService, CookbookService>()
					.AddSingleton<IMessenger, WeakReferenceMessenger>()
					.AddSingleton<INotificationEndpoint, NotificationEndpoint>()
					.AddSingleton<IRecipeEndpoint, RecipeEndpoint>()
					.AddSingleton<IUserEndpoint, UserEndpoint>()
					.AddSingleton<ICookbookEndpoint, CookbookEndpoint>();
			})

			.UseNavigation(ReactiveViewModelMappings.ViewModelMappings, RegisterRoutes, configureServices: ConfigureNavServices));

		LiveCharts.Configure(config =>
			config
			.HasMap<NutritionChartItem>((nutritionChartItem, point) =>
			{
				// here we use the index as X, and the nutrition value as Y 
				return new(point, nutritionChartItem.Value);
			})
		);
		_window = builder.Window;

		Host = await builder.NavigateAsync<ShellControl>();

		var config = Host.Services.GetRequiredService<IOptions<AppConfig>>();
		var userService = Host.Services.GetRequiredService<IUserService>();
		var themeService = _window.GetThemeService();
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
			new ViewMap<NotificationsPage, NotificationsModel>(),
			new ViewMap<ProfilePage, ProfileModel>(Data: new DataMap<User>(), ResultData: typeof(IChefEntity)),
			new ViewMap<RecipeDetailsPage, RecipeDetailsModel>(Data: new DataMap<Recipe>()),
			new ViewMap<FavoriteRecipesPage, FavoriteRecipesModel>(),
			new DataViewMap<SearchPage, SearchModel, SearchFilter>(),
			new ViewMap<SettingsPage, SettingsModel>(Data: new DataMap<User>()),
			new ViewMap<LiveCookingPage, LiveCookingModel>(Data: new DataMap<LiveCookingParameter>()),
			new ViewMap<ReviewsPage, ReviewsModel>(Data: new DataMap<ReviewParameter>()),
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
					new RouteMap("Main", View: views.FindByViewModel<MainModel>(), Nested: new RouteMap[]
					{
						#region Main Tabs
						new RouteMap("Home", View: views.FindByViewModel<HomeModel>(), IsDefault: true),
						new RouteMap("Search", View: views.FindByViewModel<SearchModel>()),
						new RouteMap("FavoriteRecipes", View: views.FindByViewModel<FavoriteRecipesModel>(), Nested: new[]
						{
							new RouteMap("MyRecipes"),
							new RouteMap("Cookbooks")
						}),
						#endregion

						#region Cookbooks
						new RouteMap("CookbookDetails", View: views.FindByViewModel<CookbookDetailModel>(), DependsOn: "FavoriteRecipes"),
						new RouteMap("UpdateCookbook", View: views.FindByViewModel<CreateUpdateCookbookModel>(), DependsOn: "FavoriteRecipes"),
						new RouteMap("CreateCookbook", View: views.FindByViewModel<CreateUpdateCookbookModel>(), DependsOn: "FavoriteRecipes"),
						#endregion

						#region Recipe Details
						new RouteMap("RecipeDetails", View: views.FindByViewModel<RecipeDetailsModel>(), DependsOn: "Home", Nested: new[] {
							new RouteMap("IngredientsTabWide"),
							new RouteMap("StepsTabWide"),
							new RouteMap("ReviewsTabWide"),
							new RouteMap("NutritionTabWide"),
							new RouteMap("IngredientsTab"),
							new RouteMap("StepsTab"),
							new RouteMap("ReviewsTab"),
							new RouteMap("NutritionTab"),
						}),
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
					new RouteMap("Notifications", View: views.FindByViewModel<NotificationsModel>(), Nested: new RouteMap[]
					{
						new RouteMap("AllTab"),
						new RouteMap("UnreadTab"),
						new RouteMap("ReadTab"),
					}),
					new RouteMap("Filter", View: views.FindByViewModel<FilterModel>()),
					new RouteMap("Reviews", View: views.FindByViewModel<ReviewsModel>()),
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
