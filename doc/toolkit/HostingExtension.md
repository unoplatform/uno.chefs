---
uid: Uno.Recipes.HostingExtensions
---

# How to setup Hosting for your app.

## Problem

Need a way to resgister service accessible throughout the application.

## Solution

Using `Uno.Extensions.Hosting` enable registration of services via dependency injection.

> [!WARNING]
> If you used the project wizard template, `app.cs` is already created and fill up for you. Follow this example, if you need to register new services to the hosting.

Instantiate `IHost`

```csharp
public static IHost? Host { get; private set; }
```

On the app launch "`OnLaunched(LaunchActivatedEventArgs args)`" use `CreateBuilder()` than `.configure()` to configure the host.

```csharp
	protected async override void OnLaunched(LaunchActivatedEventArgs args)
	{
        var builder = this.CreateBuilder(args)
        .Configure(host => host)
	}
```

At this point you can append usefull configuration to your host. Here some example from chefs App.

### UseEnvironment

Configure the environments variable of the host.

```csharp
.UseEnvironment(Environments.Development)
```

### UseLogging 

Configure logging default config. [UseLogging documentation](https://platform.uno/docs/articles/external/uno.extensions/doc/Overview/Logging/LoggingOverview.html)

```csharp
.UseLogging(configure: (context, logBuilder) =>
{
	// Configure log levels for different categories of logging
	logBuilder.SetMinimumLevel(
		context.HostingEnvironment.IsDevelopment() ?
			LogLevel.Information :
			LogLevel.Warning);
}, enableUnoLogging: true)
```

### UseConfiguration

Add global configuration. [UseConfiguration](https://platform.uno/docs/articles/external/uno.extensions/doc/Overview/Configuration/ConfigurationOverview.html)

```csharp
.UseConfiguration(configure: configBuilder =>
	configBuilder
		.EmbeddedSource<App>()
		.Section<AppConfig>()
		.Section<Credentials>()
		.Section<SearchHistory>()
)
```

### UseLocalization

Enable localization (see appsettings.json for supported languages)

```csharp
.UseLocalization()
```

### UseSerialization

Register Json serializers.

```csharp
.UseSerialization()
```

### ConfigureServices

Register Services for the app using dependency injection. see: [Use Service](https://platform.uno/docs/articles/external/uno.extensions/doc/Learn/Tutorials/DependencyInjection/HowTo-DependencyInjectionSetup.html)

``` csharp
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
```

### UseNavigation

Configure Navigation Service.

```csharp
.UseNavigation(ReactiveViewModelMappings.ViewModelMappings, RegisterRoutes, configureServices: ConfigureNavServices);
```

### Build the host

Complete the process by building the host and navigate to shellPage. ShellPage is use to initiating the app than navigate to the first page.

```csharp
Host = await builder.NavigateAsync<ShellControl>();
```

## Source Code

Chefs app

- [app](https://github.com/unoplatform/uno.chefs/blob/c39edbc737dfd899b31cb3ba24d017c9e8351861/src/Chefs/App.cs#L13)


## Documentation

- [Hosting documentation](https://platform.uno/docs/articles/external/uno.extensions/doc/Learn/Hosting/HowTo-HostingSetup.html)