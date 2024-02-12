---
uid: Uno.Recipes.SafeArea
---

# How to restrict page content to the Safe Area on mobile devices
 
## Problem
 
In a mobile platform context, UI elements can be obscured by device-specific features like status bars, rounded corners, notches, or soft-input panels like on-screen keyboards. Certain content, such as readable text or interactive controls, needs to stay within the visible screen area to ensure usability.
 
## Solution

```xml
<Page ...
      xmlns:utu="using:Uno.Toolkit.UI"
      utu:SafeArea.Insets="VisibleBounds"
      .../>
```

The above code has the following effect:
<table>
  <tr>
    <th>Without SafeArea</th>
    <th>SafeArea applied</th>
  </tr>
  <tr>
   <td><img src="../assets/without-safearea.png" width="400px" alt="SafeArea not implemented"/></td>
   <td><img src="../assets/with-safearea.png" width="400px" alt="SafeArea implemented"/></td>
  </tr>
</table>

### Source Code

- Chefs app [Welcome Page](https://github.com/unoplatform/uno.chefs/blob/main/src/Chefs/Views/WelcomePage.xaml#L17)

### Documentation

- [SafeArea documentation](https://platform.uno/docs/articles/external/uno.toolkit.ui/doc/controls/SafeArea.html)