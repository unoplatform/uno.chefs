---
uid: Uno.Recipes.ExtendedSplashScreen
---

# How to 
 
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

The above code has the following effect:
<table>
  <tr>
    <th>ExtendedSplashScreen</th>
  </tr>
  <tr>
   <td><img src="../assets/extended-splashscreen.gif" width="800px" alt="ExtendedSplashScreen Animation"/></td>
  </tr>
</table>

## Source Code

- Chefs app [ShellControl](https://github.com/unoplatform/uno.chefs/blob/c39edbc737dfd899b31cb3ba24d017c9e8351861/src/Chefs/Views/ShellControl.xaml#L12)

## Documentation

- [ExtendedSplashScreen documentation](xref:Toolkit.Controls.ExtendedSplashScreen)