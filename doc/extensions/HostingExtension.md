---
uid: Uno.Recipes.Hosting
---

# How to Build a Hosted Application

Using `Uno.Extensions.Hosting` enables registration of services via dependency injection.

In "`OnLaunched(LaunchActivatedEventArgs args)`", use `CreateBuilder()` and `.Configure()` to configure the host. The `IApplicationBuilder` has many extension methods to configure the host. Here are some of the most common ones:

```csharp
protected override async void OnLaunched(LaunchActivatedEventArgs args)
{
    var builder = this.CreateBuilder(args)
        // Add navigation support for toolkit controls such as TabBar and NavigationView
            .UseToolkitNavigation()
            .Configure(host => host
...
```

## Source Code

Chefs app

- [App.xaml.cs](https://github.com/unoplatform/uno.chefs/blob/139edc9eab65b322e219efb7572583551c40ad32/Chefs/App.xaml.cs#L29)

## Documentation

- [Hosting Documentation](xref:Uno.Extensions.Hosting.HowToHostingSetup)