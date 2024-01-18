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

			.UseNavigation(ReactiveViewModelMappings.ViewModelMappings, RegisterRoutes));

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
		var themeService = _window.GetThemeService();
		var appTheme = config.Value?.IsDark switch
		{
			true => AppTheme.Dark,
			false => AppTheme.Light,
			_ => AppTheme.System
		};

		await themeService.SetThemeAsync(appTheme);
	}

	private static void RegisterRoutes(IViewRegistry views, IRouteRegistry routes)
	{
		views.Register(
			new ViewMap(ViewModel: typeof(ShellModel)),
			new ViewMap<MainPage, MainModel>(),
			new ViewMap<WelcomePage, WelcomeModel>(),
			new ViewMap<FiltersFlyout>(),
			new ViewMap<FiltersPage, FilterModel>(Data: new DataMap<SearchFilter>()),
			new ViewMap<HomePage, HomeModel>(),
			new DataViewMap<CreateUpdateCookbookPage, CreateUpdateCookbookModel, Cookbook>(),
			new ViewMap<LoginPage, LoginModel>(ResultData: typeof(Credentials)),
			new ViewMap<NotificationsFlyout>(),
			new ViewMap<NotificationsPage, NotificationsModel>(),
			new ViewMap<ProfileFlyout>(ResultData: typeof(IChefEntity)),
			new ViewMap<ProfilePage, ProfileModel>(Data: new DataMap<User>()),
			new ViewMap<RecipeDetailsPage, RecipeDetailsModel>(Data: new DataMap<Recipe>()),
			new ViewMap<FavoriteRecipesPage, FavoriteRecipesModel>(),
			new DataViewMap<SearchPage, SearchModel, SearchFilter>(),
			new ViewMap<SettingsPage, SettingsModel>(Data: new DataMap<User>()),
			new ViewMap<LiveCookingPage, LiveCookingModel>(Data: new DataMap<LiveCookingParameter>()),
			new ViewMap<ReviewsFlyout>(),
			new ViewMap<ReviewsPage, ReviewsModel>(Data: new DataMap<ReviewParameter>()),
			new ViewMap<CookbookDetailPage, CookbookDetailModel>(Data: new DataMap<Cookbook>()),
			new ViewMap<CompletedDialog>(),
			new ViewMap<MapPage, MapModel>()
		);

		routes.Register(
			new RouteMap("", View: views.FindByViewModel<ShellModel>(),
				Nested: new RouteMap[]
				{
					new RouteMap("Welcome", View: views.FindByViewModel<WelcomeModel>()),
					new RouteMap("Login", View: views.FindByViewModel<LoginModel>()),
					new RouteMap("Main", View: views.FindByViewModel<MainModel>(), Nested: new RouteMap[]
					{
						new RouteMap("Home", View: views.FindByViewModel<HomeModel>(), IsDefault: true, Nested: new RouteMap[]
						{
							new RouteMap("Notifications", View: views.FindByView<NotificationsFlyout>(), Nested: new RouteMap[]
							{
								new RouteMap("NotificationsContent", View: views.FindByViewModel<NotificationsModel>(), IsDefault:true, Nested: new[]
								{
									new RouteMap("AllTab"),
									new RouteMap("UnreadTab"),
									new RouteMap("ReadTab"),
								})
							}),
						}),
						new RouteMap("RecipeDetails", View: views.FindByViewModel<RecipeDetailsModel>(), DependsOn: "Home"),
						new RouteMap("LiveCooking", View: views.FindByViewModel<LiveCookingModel>(), DependsOn: "RecipeDetails"),
						new RouteMap("Search", View: views.FindByViewModel<SearchModel>(), Nested: new RouteMap[]
						{
							new RouteMap("Filter", View: views.FindByView<FiltersFlyout>(), Nested: new RouteMap[]
							{
								new RouteMap("FilterContent", View: views.FindByViewModel<FilterModel>(), IsDefault:true)
							}),
						}),
						new RouteMap("FavoriteRecipes", View: views.FindByViewModel<FavoriteRecipesModel>(), Nested: new[]
						{
							new RouteMap("MyRecipes"),
							new RouteMap("Cookbooks")
						}),
						new RouteMap("CookbookDetails", View: views.FindByViewModel<CookbookDetailModel>(), DependsOn: "FavoriteRecipes"),
						new RouteMap("CookbookRecipeDetails", View: views.FindByViewModel<RecipeDetailsModel>(), DependsOn: "CookbookDetails"),
						new RouteMap("CreateUpdateCookbook", View: views.FindByViewModel<CreateUpdateCookbookModel>(), DependsOn: "CookbookDetails"),
						new RouteMap("SearchRecipeDetails", View: views.FindByViewModel<RecipeDetailsModel>(), DependsOn: "Search"),
					}),
					new RouteMap("RecipeDetails", View: views.FindByViewModel<RecipeDetailsModel>(), DependsOn: "Main", Nested: new[] {
						new RouteMap("IngredientsTabWide"),
						new RouteMap("StepsTabWide"),
						new RouteMap("ReviewsTabWide"),
						new RouteMap("NutritionTabWide"),
						new RouteMap("IngredientsTab"),
						new RouteMap("StepsTab"),
						new RouteMap("ReviewsTab"),
						new RouteMap("NutritionTab"),
					}),
					new RouteMap("Reviews", View: views.FindByView<ReviewsFlyout>(), Nested: new RouteMap[]
					{
						new RouteMap("ReviewsContent", View: views.FindByViewModel<ReviewsModel>(), DependsOn: "RecipeDetails", IsDefault:true)
					}),
					new RouteMap("FavoriteRecipeDetails", View: views.FindByViewModel<RecipeDetailsModel>()),
					new RouteMap("FavoriteCreateUpdateCookbook", View: views.FindByViewModel<CreateUpdateCookbookModel>()),
					new RouteMap("Profile", View: views.FindByView<ProfileFlyout>(), Nested: new RouteMap[]
					{
						new RouteMap("ProfileDetails", View: views.FindByViewModel<ProfileModel>(), IsDefault:true),
						new RouteMap("Settings", View: views.FindByViewModel<SettingsModel>(), DependsOn:"ProfileDetails"),
					}),
					new RouteMap("Completed", View: views.FindByView<CompletedDialog>()),
					new RouteMap("Map", View: views.FindByViewModel<MapModel>(), DependsOn: "Main")
				}
			)
		);
	}
}
