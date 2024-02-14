---
uid: Uno.Recipes.StatusBarExtension
---

# How to customize the colors of the status bar on mobile devices.

## Problem

iOS and Android provide default appearances for the status bar, but developers often need to customize it to match their app's design or theme.

## Solution

StatusBar extensions provide a quick way to set the background and foreground colors of the current page's status bar.

```xml
<Page ...
      xmlns:utu="using:Uno.Toolkit.UI"
      utu:StatusBar.Background="{ThemeResource SurfaceBrush}"
      utu:StatusBar.Foreground="Auto" />
```


For iOS add:
```xml
<!--info.plist-->
<key>UIViewControllerBasedStatusBarAppearance</key>
<false/>
```


The above code has the following effect:
<table>
  <tr>
    <th>Not set</th>
    <th>Light mode</th>
    <th>Dark mode</th>
  </tr>
  <tr>
   <td><img src="../assets/StatusBar_NotSet.png" width="600px" alt="StatusBar not set"/></td>
   <td><img src="../assets/StatusBar_light.png" width="600px" alt="StatusBar light mode"/></td>
   <td><img src="../assets/StatusBar_dark.png" width="600px" alt="StatusBar dark mode"/></td>
  </tr>
</table>

## Source Code

Chefs app
- [Welcome Page](https://github.com/unoplatform/uno.chefs/blob/f3b5a256aa7afd621389089ddea75d309e28c373/src/Chefs/Views/WelcomePage.xaml#L14-L16)
- [Map Page](https://github.com/unoplatform/uno.chefs/blob/f3b5a256aa7afd621389089ddea75d309e28c373/src/Chefs/Views/MapPage.xaml#L15-L16)
- [All references](https://github.com/search?q=repo%3Aunoplatform%2Funo.chefs+statusbar&type=code)

## Documentation

- [StatusBar documentation](xref:Toolkit.Helpers.StatusBarExtensions)