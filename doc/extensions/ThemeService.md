---
uid: Uno.Recipes.ThemeService.md
---

# Using ThemeService in Uno

## Problem

You need a way to manage and switch application themes dynamically at runtime in an Uno Platform application.

## Solution

The `ThemeService` in Uno.Extensions allows for easy management and switching of themes within your application. 

### Adding ThemeService

To integrate `ThemeService` in your Uno application, follow these steps:

#### App startup Configuration

1. Register the ThemeService in your app startup:

``` csharp
public class App : Application
{
    protected async override void OnLaunched(LaunchActivatedEventArgs args)
    {
        var builder = this.CreateBuilder(args)
            .Configure(host => host
                .UseThemeService() // Register ThemeService
            );
            // Code omitted for brevity
    }
}
```
2. Consume the ThemeService in your application:

```csharp
public partial class MainPage : Page
{
    private readonly IThemeService _themeService;

    public MainPage(IThemeService themeService)
    {
        _themeService = themeService;
        this.InitializeComponent();
    }

    private void ToggleTheme(object sender, RoutedEventArgs e)
    {
        var newTheme = _themeService.CurrentTheme == ApplicationTheme.Dark ? ApplicationTheme.Light : ApplicationTheme.Dark;
        _themeService.SetTheme(newTheme);
    }
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

## Implementation Details

The `ThemeService` implementation can be found [here](https://github.com/unoplatform/uno.extensions/blob/51c9c1ef14f686363f946588733faecc5a1863ff/src/Uno.Extensions.Core.UI/Toolkit/ThemeService.cs). It provides methods to get and set the current theme.

### Example Usage in Workshop

An example usage of the ThemeService can be found in the [Simple Calc workshop](https://platform.uno/docs/articles/external/workshops/simple-calc/modules/MVVM-XAML/05-Finish%20the%20App/README.html#adding-the-themeservice).

### Platform-Specific Customization

To customize the `ThemeService` behavior per platform, you can use platform-specific properties defined in your project file.

```xml
<PropertyGroup>
    <IsAndroid>true</IsAndroid>
    <IsIOS>false</IsIOS>
    <IsMacCatalyst>false</IsMacCatalyst>
    <IsWinAppSdk>false</IsWinAppSdk>
    <IsBrowserWasm>true</IsBrowserWasm>
</PropertyGroup>
```

## Source Code

Chefs app

- [App Startup](https://github.com/unoplatform/uno.chefs/blob/a623c4e601f705621eb9ae622aa6e0f6984ee415/src/Chefs/App.cs#L43)
