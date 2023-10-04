namespace Chefs
{
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
						.AddSingleton<ICookbookEndpoint, CookbookEndpoint>()
						.AddSingleton<IMapService, MapService>();
				})

				.UseNavigation(ReactiveViewModelMappings.ViewModelMappings, RegisterRoutes));

			_window = builder.Window;

			Host = await builder.NavigateAsync<ShellControl>();
		}

		private static void RegisterRoutes(IViewRegistry views, IRouteRegistry routes)
		{
			views.Register(
				new ViewMap(ViewModel: typeof(ShellModel)),
				new ViewMap<MainPage, MainModel>(),
				new ViewMap<WelcomePage, WelcomeModel>(),
				new ViewMap<FilterPage>(),
				new ViewMap<FilterContentPage, FilterModel>(Data: new DataMap<SearchFilter>()),
				new ViewMap<HomePage, HomeModel>(),
				new DataViewMap<CreateUpdateCookbookPage, CreateUpdateCookbookModel, Cookbook>(),
				new ViewMap<LoginPage, LoginModel>(ResultData: typeof(Credentials)),
				new ViewMap<NotificationsPage>(),
				new ViewMap<NotificationsContentPage, NotificationsModel>(),
				new ViewMap<ProfilePage>(ResultData: typeof(IChefEntity)),
				new ViewMap<ProfileDetailsPage, ProfileModel>(Data: new DataMap<User>()),
				new ViewMap<RecipeDetailsPage, RecipeDetailsModel>(Data: new DataMap<Recipe>()),
				new ViewMap<SavedRecipesPage, SavedRecipesModel>(),
				new DataViewMap<SearchPage, SearchModel, SearchFilter>(),
				new ViewMap<SettingsPage, SettingsModel>(Data: new DataMap<User>()),
				new ViewMap<LiveCookingPage, LiveCookingModel>(Data: new DataMap<LiveCookingParameter>()),
				new ViewMap<ReviewsPage>(),
				new ViewMap<ReviewsContentPage, ReviewsModel>(Data: new DataMap<ReviewParameter>()),
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
								new RouteMap("Notifications", View: views.FindByView<NotificationsPage>(), Nested: new RouteMap[]
								{
									new RouteMap("NotificationsContent", View: views.FindByViewModel<NotificationsModel>(), IsDefault:true)
								}),
							}),
							new RouteMap("RecipeDetails", View: views.FindByViewModel<RecipeDetailsModel>(), DependsOn: "Home"),
							new RouteMap("Search", View: views.FindByViewModel<SearchModel>(), DependsOn:"Home", Nested: new RouteMap[]
							{
								new RouteMap("Filter", View: views.FindByView<FilterPage>(), Nested: new RouteMap[]
								{
									new RouteMap("FilterContent", View: views.FindByViewModel<FilterModel>(), IsDefault:true)
								}),
							}),
							new RouteMap("SavedRecipes", View: views.FindByViewModel<SavedRecipesModel>()),
							new RouteMap("SavedRecipeDetails", View: views.FindByViewModel<RecipeDetailsModel>(), DependsOn: "SavedRecipes"),
							new RouteMap("SavedCreateUpdateCookbook", View: views.FindByViewModel<CreateUpdateCookbookModel>(), DependsOn: "SavedRecipes"),
							new RouteMap("CookbookDetails", View: views.FindByViewModel<CookbookDetailModel>(), DependsOn: "SavedRecipes"),
							new RouteMap("CreateUpdateCookbook", View: views.FindByViewModel<CreateUpdateCookbookModel>(), DependsOn: "CookbookDetails"),
							new RouteMap("CookbookRecipeDetails", View: views.FindByViewModel<RecipeDetailsModel>(), DependsOn: "CookbookDetails"),
							new RouteMap("SearchRecipeDetails", View: views.FindByViewModel<RecipeDetailsModel>(), DependsOn: "Search"),
							new RouteMap("LiveCooking", View: views.FindByViewModel<LiveCookingModel>(), DependsOn: "RecipeDetails"),
							new RouteMap("Reviews", View: views.FindByView<ReviewsPage>(), Nested: new RouteMap[]
							{
								new RouteMap("ReviewsContent", View: views.FindByViewModel<ReviewsModel>(), DependsOn: "RecipeDetails", IsDefault:true)
							}),

						}),
						new RouteMap("Profile", View: views.FindByView<ProfilePage>(), Nested: new RouteMap[]
						{
							new RouteMap("ProfileDetails", View: views.FindByViewModel<ProfileModel>(), IsDefault:true),
							new RouteMap("Settings", View: views.FindByViewModel<SettingsModel>(), DependsOn:"ProfileDetails"),
						}),
						new RouteMap("Completed", View: views.FindByView<CompletedDialog>()),
						new RouteMap("Map", View: views.FindByViewModel<MapModel>())
					}
				)
			);
		}
	}
}