using Chefs.Business;
using Chefs.Data;
using CommunityToolkit.Mvvm.Messaging;
using Chefs.Settings;
using Microsoft.Extensions.DependencyInjection;

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
                    .AddSingleton<IMessenger, WeakReferenceMessenger>()
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
            new ViewMap(ViewModel: typeof(ShellModel)),
            new ViewMap<MainPage, MainModel>(),
            new ViewMap<WelcomePage, WelcomeModel>(),
            new ViewMap<RegisterPage, RegisterModel>(),
            new ViewMap<FilterPage, FilterModel>(Data: new DataMap<SearchFilter>()),
            new ViewMap<HomePage, HomeModel>(),
            new ViewMap<IngredientsPage, IngredientsModel>(Data: new DataMap<IngredientsParameter>()),
            new ViewMap<CreateCookbookPage, CreateCookbookModel>(),
            new DataViewMap<UpdateCookbookPage, UpdateCookbookModel, Cookbook>(),
            new ViewMap<LoginPage, LoginModel>(ResultData: typeof(Credentials)),
            new ViewMap<NotificationsPage, NotificationsModel>(),
            new ViewMap<ProfilePage, ProfileModel>(Data: new DataMap<User>()),
            new ViewMap<RecipeDetailsPage, RecipeDetailsModel>(Data: new DataMap<Recipe>()),
            new ViewMap<SavedRecipesPage, SavedRecipesModel>(),
            new DataViewMap<SearchPage, SearchModel, SearchFilter>(),
            new ViewMap<SettingsPage, SettingsModel>(Data: new DataMap<User>()),
            new ViewMap<LiveCookingPage, LiveCookingModel>(Data: new DataMap<LiveCookingParameter>()),
            new ViewMap<ReviewsPage, ReviewsModel>(Data: new DataMap<ReviewParameter>()),
            new ViewMap<CookbookDetailPage, CookbookDetailModel>(Data: new DataMap<Cookbook>()),
            new ViewMap<CompletedDialog>()
        );

        routes
            .Register(
                new RouteMap("", View: views.FindByViewModel<ShellModel>(),
                        Nested: new RouteMap[]
                        {
                            new RouteMap("Welcome", View: views.FindByViewModel<WelcomeModel>()),
                            new RouteMap("Login", View: views.FindByViewModel<LoginModel>()),
                            new RouteMap("Register", View: views.FindByViewModel<RegisterModel>()),
                            new RouteMap("Main", View: views.FindByViewModel<MainModel>(), Nested: new RouteMap[]
                            {
                                new RouteMap("Home", View: views.FindByViewModel<HomeModel>(), IsDefault: true, Nested: new RouteMap[]
                                {
                                    new RouteMap("Notifications", View: views.FindByViewModel<NotificationsModel>())
                                }),
                                new RouteMap("RecipeDetails", View: views.FindByViewModel<RecipeDetailsModel>(), DependsOn: "Home"),
                                new RouteMap("Search", View: views.FindByViewModel<SearchModel>(), DependsOn:"Home", Nested: new RouteMap[]
                                {
                                    new RouteMap("Filter", View: views.FindByViewModel<FilterModel>())
                                }),
                                new RouteMap("SavedRecipes", View: views.FindByViewModel<SavedRecipesModel>()),
                                new RouteMap("SavedRecipeDetails", View: views.FindByViewModel<RecipeDetailsModel>(), DependsOn: "SavedRecipes"),
                                new RouteMap("SavedUpdateCookbook", View: views.FindByViewModel<UpdateCookbookModel>(), DependsOn: "SavedRecipes"),
                                new RouteMap("CookbookDetails", View: views.FindByViewModel<CookbookDetailModel>(), DependsOn: "SavedRecipes"),
                                new RouteMap("UpdateCookbook", View: views.FindByViewModel<UpdateCookbookModel>(), DependsOn: "CookbookDetails"),
                                new RouteMap("CreateCookbook", View: views.FindByViewModel<CreateCookbookModel>(), DependsOn:"SavedRecipes"),
                                new RouteMap("CookbookRecipeDetails", View: views.FindByViewModel<RecipeDetailsModel>(), DependsOn: "CookbookDetails"),
                                new RouteMap("Ingredients", View: views.FindByViewModel<IngredientsModel>(), DependsOn: "RecipeDetails"),
                                new RouteMap("LiveCooking", View: views.FindByViewModel<LiveCookingModel>(), DependsOn: "RecipeDetails"),
                                new RouteMap("Reviews", View: views.FindByViewModel<ReviewsModel>(), DependsOn: "RecipeDetails"),
                                new RouteMap("Profile", View: views.FindByViewModel<ProfileModel>()),
                                new RouteMap("Settings", View: views.FindByViewModel<SettingsModel>()),
                            }),
                            new RouteMap("Completed", View: views.FindByView<CompletedDialog>()),
                        }));
    }
}
