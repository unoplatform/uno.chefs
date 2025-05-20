---
uid: Uno.Recipes.ThemeService
---

# How to Handle Theme Switching

## Problem

Currently, there is no way to switch application themes at runtime from any layer, including view models. There is also a need to have a way to store the current theme and be able to initialize the app to the persisted theme preference.

## Solution

The **Uno.Extensions** library addresses this problem by providing an injectable implementation of an `IThemeService` interface that can be registered as part of the `IHostBuilder` from `Uno.Extensions.Hosting`.

### Adding ThemeService

To integrate `ThemeService` in your Uno application, follow these steps:

#### App Startup Configuration

1. Register the `IThemeService` in your app startup:

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

1. Consume the ThemeService in your view model:

```csharp
public partial record SettingsModel
{
    private readonly IThemeService _themeService;

    public SettingsModel(IThemeService themeService)
    {
         _themeService = themeService;
    }

    public IList<AppTheme> ThemeOptions => Enum.GetValues(typeof(AppTheme)).Cast<AppTheme>().ToList();

    public IState<AppTheme> Theme => State
        .Value(this, () => _themeService.Theme)
        .ForEach(async (theme, _) => await _themeService.SetThemeAsync(theme));
}
```

1. Use the `Theme` `IState` in your XAML to bind to the current theme and allow the user to change it:

```xml
<ComboBox ItemsSource="{Binding ThemeOptions}"
          SelectedItem="{Binding Theme, Mode=TwoWay}"
          HorizontalAlignment="Right" />
```

## Source Code

- [SettingsPage](https://github.com/unoplatform/uno.chefs/blob/47c1f7342a7f312719761f2089e3828d587e5c64/Chefs/Views/SettingsPage.xaml)
- [SettingsModel](https://github.com/unoplatform/uno.chefs/blob/47c1f7342a7f312719761f2089e3828d587e5c64/Chefs/Presentation/SettingsModel.cs)
