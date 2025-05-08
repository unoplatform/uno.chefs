---
uid: Uno.Recipes.ExtendedSplashScreen
---

# Extending Splash Screen Duration for Custom Loading

## Problem

When an application is launched, it may take some time for the user interface to fully load, especially if the application is complex or requires significant resources. This can lead to a blank or unresponsive screen, which can be confusing or frustrating for the user.

## Solution

The `ExtendedSplashScreen` control extends the native splash screen for an additional amount of time and can be used to display a custom loading screen while the application is loading.

```xml
<utu:ExtendedSplashScreen x:Name="Splash">
    <utu:ExtendedSplashScreen.LoadingContentTemplate>
        <DataTemplate>
        ...
        </DataTemplate>
    </utu:ExtendedSplashScreen.LoadingContentTemplate>
</utu:ExtendedSplashScreen>
```

```csharp
public class ShellModel
{
    private readonly INavigator _navigator;

    public ShellModel(INavigator navigator)
    {
      // Simulating loading time for extended splash screen
      System.Threading.Thread.Sleep(5000);

      _navigator = navigator;
      _ = Start();
    }

    public async Task Start() => await _navigator.NavigateViewModelAsync<WelcomeModel>(this);
}
```

The above code results in the splash screen being extended for 5 seconds:

![ExtendedSplashScreen Animation](../assets/extended-splashscreen.gif)

Alternatively, when using Navigation, you can use the Navigate callback to add a delay before navigating to the entry point of the application. See an example in the [Extensions Playground](https://github.com/unoplatform/uno.extensions/blob/f3348bd95b5fa58155e8e34a6154acd3362559f7/samples/Playground/Playground/App.cs#L107).

## Source Code

- [ShellControl.xaml](https://github.com/unoplatform/uno.chefs/blob/139edc9eab65b322e219efb7572583551c40ad32/Chefs/Views/ShellControl.xaml#L12)

## Documentation

- [ExtendedSplashScreen documentation](xref:Toolkit.Controls.ExtendedSplashScreen)
