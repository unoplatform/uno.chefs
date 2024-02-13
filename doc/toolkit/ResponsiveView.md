---
uid: Uno.Recipes.ResponsiveView
---

# How to create responsive layouts in XAML

## Problem

XAML currently lacks a simple and flexible approach for creating responsive layouts. While options like `VisualStates` with `AdaptiveTriggers` exist, they become cumbersome for complex scenarios involving significant UI structure changes based on screen size.

## Solution

<table>
  <tr>
    <th>Typical scenario</th>
  </tr>
  <tr>
   <td><img src="../assets/responsiveview-sample.gif" width="1200px" alt="ResponsiveView Scenario"/></td>
  </tr>
</table>

`ResponsiveView` is conceived with more complex scenarios in mind, where the UI structure changes significantly based on screen size. For a lightweight approach, consider using the [`ResponsiveExtension`](xref:uno.recipes.responsiveextension).

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

Chefs app
- [Welcome Page](https://github.com/unoplatform/uno.chefs/blob/c39edbc737dfd899b31cb3ba24d017c9e8351861/src/Chefs/Views/WelcomePage.xaml#L59)
- [Recipe Details Page](https://github.com/unoplatform/uno.chefs/blob/c39edbc737dfd899b31cb3ba24d017c9e8351861/src/Chefs/Views/RecipeDetailsPage.xaml#L74)

### Documentation

- [ResponsiveView documentation](xref:Toolkit.Controls.ResponsiveView)