
using Chefs.Business;
using Chefs.Data;
using Chefs.Presentation;
using Chefs.Settings;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Immutable;
using Uno.Extensions.Configuration;
using Uno.Extensions.Hosting;

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
                        .Section<SearchHistory>()
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
            new ViewMap(ViewModel: typeof(ShellViewModel)),
            new ViewMap<MainPage, MainViewModel>(),
            new ViewMap<WelcomePage, WelcomeViewModel>(),
            new ViewMap<RegisterPage, RegisterViewModel>(),
            new ViewMap<FilterPage, FilterViewModel>(Data: new DataMap<SearchFilter>()),
            new ViewMap<HomePage, HomeViewModel>(),
            new ViewMap<IngredientsPage, IngredientsViewModel>(Data: new DataMap<IngredientsParameter>()),
            new ViewMap<CreateCookbookPage, CreateCookbookViewModel>(),
            new DataViewMap<UpdateCookbookPage, UpdateCookbookViewModel, Cookbook>(),
            new ViewMap<LoginPage, LoginViewModel>(ResultData: typeof(Credentials)),
            new ViewMap<NotificationsPage, NotificationsViewModel>(),
            new ViewMap<ProfilePage, ProfileViewModel>(Data: new DataMap<User>()),
            new ViewMap<RecipeDetailsPage, RecipeDetailsViewModel>(Data: new DataMap<Recipe>()),
            new ViewMap<SavedRecipesPage, SavedRecipesViewModel>(),
            new DataViewMap<SearchPage, SearchViewModel, SearchFilter>(),
            new ViewMap<SettingsPage, SettingsViewModel>(Data: new DataMap<User>()),
            new ViewMap<LiveCookingPage, LiveCookingViewModel>(Data: new DataMap<LiveCookingParameter>()),
            new ViewMap<ReviewsPage, ReviewsViewModel>(Data: new DataMap<ReviewParameter>()),
            new ViewMap<CookbookDetailPage, CookbookDetailViewModel>(Data: new DataMap<Cookbook>())
        );

        routes
            .Register(
                new RouteMap("", View: views.FindByViewModel<ShellViewModel>(),
                        Nested: new RouteMap[]
                        {
                            new RouteMap("Welcome", View: views.FindByViewModel<WelcomeViewModel>()),
                            new RouteMap("Login", View: views.FindByViewModel<LoginViewModel>()),
                            new RouteMap("Register", View: views.FindByViewModel<RegisterViewModel>()),
                            new RouteMap("Main", View: views.FindByViewModel<MainViewModel>(), Nested: new RouteMap[]
                            {
                                new RouteMap("Home", View: views.FindByViewModel<HomeViewModel>(), IsDefault: true, Nested: new RouteMap[]
                                {
                                    new RouteMap("Notifications", View: views.FindByViewModel<NotificationsViewModel>()),
                                }),
                                new RouteMap("Profile", View: views.FindByViewModel<ProfileViewModel>()),
                                new RouteMap("Settings", View: views.FindByViewModel<SettingsViewModel>()),
                                new RouteMap("ProfileCookbookDetail", View: views.FindByViewModel<CookbookDetailViewModel>(), DependsOn: "Profile"),
                                new RouteMap("ProfileUpdateCookbook", View: views.FindByViewModel<UpdateCookbookViewModel>(), DependsOn: "ProfileCookbookDetail"),
                                new RouteMap("Search", View: views.FindByViewModel<SearchViewModel>(), DependsOn:"Home", Nested: new RouteMap[]
                                {
                                    new RouteMap("Filter", View: views.FindByViewModel<FilterViewModel>())
                                }),
                                new RouteMap("SavedRecipes", View: views.FindByViewModel<SavedRecipesViewModel>()),
                                new RouteMap("CreateCookbook", View: views.FindByViewModel<CreateCookbookViewModel>(), DependsOn: "SavedRecipes"),
                                new RouteMap("SavedCookbookDetail", View: views.FindByViewModel<CookbookDetailViewModel>(), DependsOn: "SavedRecipes"),
                                new RouteMap("CreateUpdateCookbook", View: views.FindByViewModel<UpdateCookbookViewModel>(), DependsOn: "SavedCookbookDetail"),
                                new RouteMap("RecipeDetails", View: views.FindByViewModel<RecipeDetailsViewModel>(), Nested: new RouteMap[]
                                {
                                    new RouteMap("Ingredients", View: views.FindByViewModel<IngredientsViewModel>(), DependsOn:"RecipeDetails"),
                                    new RouteMap("LiveCooking", View: views.FindByViewModel<LiveCookingViewModel>(), DependsOn:"RecipeDetails"),
                                    new RouteMap("Reviews", View: views.FindByViewModel<ReviewsViewModel>(), DependsOn:"RecipeDetails"),
                                }),
                            })
                        }));
    }
}
