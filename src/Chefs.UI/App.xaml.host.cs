
using Chefs.Presentation;

namespace Chefs;

public sealed partial class App : Application
{
	private IHost Host { get; } = BuildAppHost();

	private static IHost BuildAppHost()
	{
		return UnoHost
				.CreateDefaultBuilder()
#if DEBUG
				// Switch to Development environment when running in DEBUG
				.UseEnvironment(Environments.Development)
#endif
				// Add platform specific log providers
				.UseLogging(configure: (context, logBuilder) =>
				{
					// Configure log levels for different categories of logging
					logBuilder
							.SetMinimumLevel(
								context.HostingEnvironment.IsDevelopment() ?
									LogLevel.Information :
									LogLevel.Warning);
				})

				.UseConfiguration(configure: configBuilder =>
					configBuilder
						.EmbeddedSource<App>()
						.Section<AppConfig>()
				)

				// Enable localization (see appsettings.json for supported languages)
				.UseLocalization()

				// Register Json serializers (ISerializer and ISerializer)
				.UseSerialization()

				// Register services for the application
				.ConfigureServices(services =>
				{
					// TODO: Register your services
					//services.AddSingleton<IMyService, MyService>();
				})


				// Enable navigation, including registering views and viewmodels
				.UseNavigation(ReactiveViewModelMappings.ViewModelMappings, RegisterRoutes)

				// Add navigation support for toolkit controls such as TabBar and NavigationView
				.UseToolkitNavigation()

				.Build(enableUnoLogging: true);

	}
	private static void RegisterRoutes(IViewRegistry views, IRouteRegistry routes)
	{
        views.Register(
            new ViewMap<ShellControl, ShellViewModel>(),
            new ViewMap<MainPage, MainViewModel>(),
            new ViewMap<SecondPage, SecondViewModel>(),
            new ViewMap<WelcomePage, WelcomeViewModel>(),
            new ViewMap<FilterPage, FilterViewModel>(),
            new ViewMap<HomePage, HomeViewModel>(),
            new ViewMap<IngredientsPage, IngredientsViewModel>(),
            new ViewMap<LoginPage, LoginViewModel>(),
            new ViewMap<NotificationsPage, NotificationsViewModel>(),
            new ViewMap<ProfilePage, ProfileViewModel>(),
            new ViewMap<RecipeDetailsPage, RecipeDetailsViewModel>(),
            new ViewMap<SavedRecipesPage, SavedRecipesViewModel>(),
            new ViewMap<SearchPage, SearchViewModel>(),
            new ViewMap<SettingsPage, SettingsViewModel>(),
            new ViewMap<StartCookingPage, StartCookingViewModel>()
            );

        routes
            .Register(
                new RouteMap("", View: views.FindByViewModel<ShellViewModel>(),
                        Nested: new RouteMap[]
                        {
                            new RouteMap("Main", View: views.FindByViewModel<MainViewModel>()),
                            new RouteMap("Second", View: views.FindByViewModel<SecondViewModel>()),
                            new RouteMap("Welcome", View: views.FindByViewModel<WelcomeViewModel>()),
                            new RouteMap("Filter", View: views.FindByViewModel<FilterViewModel>()),
                            new RouteMap("Home", View: views.FindByViewModel<HomeViewModel>()),
                            new RouteMap("Ingredients", View: views.FindByViewModel<IngredientsViewModel>()),
                            new RouteMap("Login", View: views.FindByViewModel<LoginViewModel>()),
                            new RouteMap("Notifications", View: views.FindByViewModel<NotificationsViewModel>()),
                            new RouteMap("Profile", View: views.FindByViewModel<ProfileViewModel>()),
                            new RouteMap("Recipe", View: views.FindByViewModel<RecipeDetailsViewModel>()),
                            new RouteMap("SavedRecipes", View: views.FindByViewModel<SavedRecipesViewModel>()),
                            new RouteMap("Search", View: views.FindByViewModel<SearchViewModel>()),
                            new RouteMap("Settings", View: views.FindByViewModel<SettingsViewModel>()),
                            new RouteMap("StartCooking", View: views.FindByViewModel<StartCookingViewModel>())
                        }));
    }
}
