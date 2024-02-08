# SafeArea
 
## Problem
 
UI elements can be obscured by device-specific features like status bars, rounded corners, notches, or soft-input panels like on-screen keyboards. Certain content, such as readable text or interactive controls, needs to stay within the visible screen area to ensure usability.
 
## Solution
 
Use the `SafeArea` control to adjust the `Padding` or `Margin` of child elements to keep content within the visible screen area, defined by `ApplicationView.VisibleBounds`.
 
You can specify which edges of the screen should be considered "safe" using `InsetMask` values (e.g., `Left`, `Right`, `Top`, `Bottom`, `SoftInput`). Using the `Mode` property, you can determine whether the insets are applied to `Padding` or `Margin`, affecting how content adapts to "unsafe" areas.
 
## Discussion
 
You can use `SafeArea` directly as a control or as an attached property on a `FrameworkElement`. This choice influences how content is displayed relative to "unsafe" areas.
 
In the [Chefs app](https://github.com/unoplatform/uno.chefs/blob/main/src/Chefs/Views/WelcomePage.xaml#L17), `SafeArea` is used as an attached property on the `Page` control of the `WelcomePage` to ensure that its content is not obscured by the status bar or device notches:
 
```xml
<Page ...
      xmlns:utu="using:Uno.Toolkit.UI"
      utu:SafeArea.Insets="VisibleBounds"
      .../>
```

This has the following effect:
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

For more usage examples, please refer to the Uno Toolkit UI [SafeArea documentation](https://platform.uno/docs/articles/external/uno.toolkit.ui/doc/controls/SafeArea.html).





