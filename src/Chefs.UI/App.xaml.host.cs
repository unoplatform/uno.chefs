
using Chefs.Business;
using Chefs.Data;
using Chefs.Presentation;
using Chefs.Settings;
using Microsoft.Extensions.DependencyInjection;
using Uno.Extensions.Configuration;

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
						.Section<Credentials>()
				)

				// Enable localization (see appsettings.json for supported languages)
				.UseLocalization()

				// Register Json serializers (ISerializer and ISerializer)
				.UseSerialization()

				// Register services for the application
				.ConfigureServices(services =>
				{
					// TODO: Register your services
					services
					.AddSingleton<INotificationService, NotificationService>()
                    .AddSingleton<IRecipeService, RecipeService>()
                    .AddSingleton<IUserService, UserService>()
                    .AddSingleton<ICookbookService, CookbookService>()

                    .AddSingleton<INotificationEndpoint, NotificationEndpoint>()
                    .AddSingleton<IRecipeEndpoint, RecipeEndpoint>()
                    .AddSingleton<IUserEndpoint, UserEndpoint>()
                    .AddSingleton<ICookbookEndpoint, CookbookEndpoint>();
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
            new ViewMap<WelcomePage, WelcomeViewModel>(),
            new ViewMap<FilterPage, FilterViewModel>(Data: new DataMap<SearchFilter>()),
            new ViewMap<HomePage, HomeViewModel>(Data: new DataMap<Credentials>()),
            new ViewMap<IngredientsPage, IngredientsViewModel>(),
            new ViewMap<LoginPage, LoginViewModel>(ResultData: typeof(Credentials)),
            new ViewMap<NotificationsPage, NotificationsViewModel>(),
            new ViewMap<ProfilePage, ProfileViewModel>(),
            new ViewMap<RecipeDetailsPage, RecipeDetailsViewModel>(),
            new ViewMap<SavedRecipesPage, SavedRecipesViewModel>(),
            new ViewMap<SearchPage, SearchViewModel>(),
            new ViewMap<SettingsPage, SettingsViewModel>(),
            new ViewMap<LiveCookingPage, LiveCookingViewModel>()
            );

        routes
            .Register(
                new RouteMap("", View: views.FindByViewModel<ShellViewModel>(),
                        Nested: new RouteMap[]
                        {
                            new RouteMap("Welcome", View: views.FindByViewModel<WelcomeViewModel>()),
                            new RouteMap("Login", View: views.FindByViewModel<LoginViewModel>()),
                            new RouteMap("RecipeDetails", View: views.FindByViewModel<RecipeDetailsViewModel>(), Nested: new RouteMap[]
                            {
                                new RouteMap("Ingredients", View: views.FindByViewModel<IngredientsViewModel>()),
                                new RouteMap("LiveCooking", View: views.FindByViewModel<LiveCookingViewModel>())
                            }),
                            new RouteMap("Main", View: views.FindByViewModel<MainViewModel>(), Nested: new RouteMap[]
                            {
                                new RouteMap("Home", View: views.FindByViewModel<HomeViewModel>(), Nested: new RouteMap[]
                                {
                                    new RouteMap("Profile", View: views.FindByViewModel<ProfileViewModel>(), Nested: new RouteMap[]
                                    {
                                        new RouteMap("Settings", View: views.FindByViewModel<SettingsViewModel>())
                                    }),
                                    new RouteMap("Notifications", View: views.FindByViewModel<NotificationsViewModel>()),
                                    new RouteMap("Search", View: views.FindByViewModel<SearchViewModel>(), Nested: new RouteMap[]
                                    {
                                        new RouteMap("Filter", View: views.FindByViewModel<FilterViewModel>())
                                    })
                                }),
                                new RouteMap("SavedRecipes", View: views.FindByViewModel<SavedRecipesViewModel>())
                            })
                        }));
    }
}
