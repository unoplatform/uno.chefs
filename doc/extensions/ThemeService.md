---
uid: Uno.Recipes.ThemeService
---

# How to change the app theme from anywhere

## Problem

Currently, there is no way to switch application themes at runtime from any layer, including view models. There is also a need to have a way to store the current theme and be able to initialize the app to the persisted theme preference.

## Solution

The **Uno.Extensions** library addresses this problem by providing an injectable implementation of an `IThemeService` interface that can be registered as part of the `IHostBuilder` from `Uno.Extensions.Hosting`.

### Adding ThemeService

To integrate `ThemeService` in your Uno application, follow these steps:

#### App startup Configuration

1. Register the ThemeService in your app startup:

``` csharp
public partial class App : Application
{
    protected async override void OnLaunched(LaunchActivatedEventArgs args)
    {
        var builder = this.CreateBuilder(args)
            .Configure(host => host
                .UseThemeSwitching()
            );
            // Code omitted for brevity
    }
}
```
2. Consume the ThemeService in your application:

```csharp
public class SettingsModel
{
    private readonly IThemeService _themeService;

    public SettingsModel(IThemeService themeService)
    {
        _themeService = themeService;

        Settings.ForEachAsync(async (settings, ct) =>
        {
            if (settings is { })
            {
                var isDark = (settings.IsDark ?? false);
                await _themeService.SetThemeAsync(isDark ? AppTheme.Dark : AppTheme.Light);
            }
        });
    }

    public IState<AppConfig> Settings { get; set; }
}
```
3. Using ThemeService in ViewModels:
```csharp
public partial class MainViewModel : ObservableObject
{
    private readonly IThemeService _themeService;

    public MainViewModel(IThemeService themeService)
    {
        _themeService = themeService;
        IsDark = _themeService.IsDark;
        _themeService.ThemeChanged += (_, _) => IsDark = _themeService.IsDark;
    }

    private bool _isDark;
    public bool IsDark
    {
        get => _isDark;
        set
        {
            if (SetProperty(ref _isDark, value))
            {
                _themeService.SetThemeAsync(value ? AppTheme.Dark : AppTheme.Light);
            }
        }
    }
}
```

### Example Usage in Workshop

An example usage of the ThemeService can be found in the [Simple Calc workshop](https://platform.uno/docs/articles/external/workshops/simple-calc/modules/MVVM-XAML/05-Finish%20the%20App/README.html#adding-the-themeservice).

## Source Code

Chefs app

- [App Startup](https://github.com/unoplatform/uno.chefs/blob/a623c4e601f705621eb9ae622aa6e0f6984ee415/src/Chefs/App.cs#L43)
