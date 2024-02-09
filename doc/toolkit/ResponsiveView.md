# How to streamline complex responsive layouts in XAML

## Problem

XAML currently lacks a simple and flexible approach for creating responsive layouts. While options like `VisualStates` with `AdaptiveTriggers` exist, they become cumbersome for complex scenarios involving significant UI structure changes based on screen size.

For example, switching between a navigation pane with a compact UI for smaller screens and a tab-based layout for larger displays requires intricate setups using multiple named containers, visibility states, and triggers.

## Solution

The `ResponsiveView` control simplifies responsive design by using a **template-based approach** and **dynamic template selection**. In addition, **error handling** is implemented to guarantee content visibility even on unexpected screen dimensions.

<table>
  <tr>
    <th>Typical scenario</th>
  </tr>
  <tr>
   <td><img src="../assets/responsiveview-sample.gif" width="1200px" alt="ResponsiveView Scenario"/></td>
  </tr>
</table>

`ResponsiveView` is conceived with more complex scenarios in mind, where the UI structure changes significantly based on screen size. For a lightweight approach, consider using the [`ResponsiveExtension`](https://platform.uno/docs/articles/external/uno.toolkit.ui/doc/helpers/responsive-extension.html).

```xml
<utu:ResponsiveView>
    <utu:ResponsiveView.NarrowTemplate>
        <DataTemplate>
        ...
        </DataTemplate>
    </utu:ResponsiveView.NarrowTemplate>

    <utu:ResponsiveView.WideTemplate>
        <DataTemplate>
        ...
        </DataTemplate>
    </utu:ResponsiveView.WideTemplate>
</utu:ResponsiveView>
```

The above code has the following effect:
<table>
  <tr>
    <th>Narrow Mode</th>
    <th>Wide Mode</th>
  </tr>
  <tr>
   <td><img src="../assets/responsiveview-narrow.png" width="400px" alt="ResponsiveView Narrow"/></td>
   <td><img src="../assets/responsiveview-wide.png" width="800px" alt="ResponsiveView Wide"/></td>
  </tr>
</table>

### Source Code

- Chefs app [Welcome Page](https://github.com/unoplatform/uno.chefs/blob/main/src/Chefs/Views/WelcomePage.xaml#L59)
- Chefs app [Recipe Details Page](https://github.com/unoplatform/uno.chefs/blob/main/src/Chefs/Views/RecipeDetailsPage.xaml#L74)

### Documentation

- Uno Toolkit UI [ResponsiveView documentation](https://platform.uno/docs/articles/external/uno.toolkit.ui/doc/controls/ResponsiveView.html)