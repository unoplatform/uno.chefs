---
uid: Uno.Recipes.Hosting
---

# How to Build a Hosted Application

Using `Uno.Extensions.Hosting` enables registration of services via dependency injection.

In "`OnLaunched(LaunchActivatedEventArgs args)`", use `CreateBuilder()` to create an `IApplicationBuilder`. It has many extension methods to configure the host. In Chefs, we configure the host in a separate file: **App.xaml.host.cs**.

In App.xaml.cs:

```csharp
protected override async void OnLaunched(LaunchActivatedEventArgs args)
{
    ...

    var builder = this.CreateBuilder(args);
    ConfigureAppBuilder(builder);
    MainWindow = builder.Window;

    ...

    Host = await builder.NavigateAsync<ShellControl>();
    Shell = MainWindow.Content as ShellControl;
    
    ...
}
```

In App.xaml.host.cs:

```csharp
private void ConfigureAppBuilder(IApplicationBuilder builder)
{
    builder
        .Configure(host => host
            ...
        )

    ...
}
```

## Source Code

Chefs app

- [App.xaml.cs](https://github.com/unoplatform/uno.chefs/blob/04a93886dd0b530386997179b80453a59e832fbe/Chefs/App.xaml.cs#L38)
- [App.xaml.host.cs](https://github.com/unoplatform/uno.chefs/blob/04a93886dd0b530386997179b80453a59e832fbe/Chefs/App.xaml.host.cs#L12-L88)

## Documentation

- [Hosting Documentation](xref:Uno.Extensions.Hosting.HowToHostingSetup)
