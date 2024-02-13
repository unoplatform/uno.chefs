---
uid: Uno.Recipes.ResponsiveExtension
---

# How to adapt properties based on screen size in XAML

## Problem

XAML currently lacks a simple and flexible approach for creating responsive layouts. While options like `VisualStates` with `AdaptiveTriggers` exist, they can become cumbersome even in simple scenarios that do not involve significant UI structure changes based on screen size.

## Solution

The `ResponsiveExtension` class is a markup extension that enables the customization of `UIElement` properties based on screen size.

<table>
  <tr>
    <th>Typical scenario</th>
  </tr>
  <tr>
   <td><img src="../assets/responsiveview-sample.gif" width="800px" alt="ResponsiveView Scenario"/></td>
  </tr>
</table>

`ResponsiveExtension` is conceived as a lightweight approach to responsiveness. For more complex scenarios, where the UI structure changes significantly based on screen size, consider using the [`ResponsiveView`](xref:uno.recipes.responsiveview) control.

```xml
<utu:AutoLayout Padding="{utu:Responsive Narrow='16,24', Wide='40,40,40,24'}">
                ...
</utu:AutoLayout>
```

The above code has the following effect:
<table>
  <tr>
    <th>Responsive Markup Extension</th>
  </tr>
  <tr>
   <td><img src="../assets/responsiveextension-animated.gif" width="800px" alt="Responsive Extension Animation"/></td>
  </tr>
</table>

### Source Code

Chefs app
- [Main Page](https://github.com/unoplatform/uno.chefs/blob/main/src/Chefs/Views/MainPage.xaml#L44)
- [Search Page](https://github.com/unoplatform/uno.chefs/blob/main/src/Chefs/Views/SearchPage.xaml#L148)
- [Home Page](https://github.com/unoplatform/uno.chefs/blob/main/src/Chefs/Views/HomePage.xaml#L290)
- [Recipe Details Page](https://github.com/unoplatform/uno.chefs/blob/main/src/Chefs/Views/RecipeDetailsPage.xaml#L24)
- [Create/Update Cookbook Page](https://github.com/unoplatform/uno.chefs/blob/main/src/Chefs/Views/CreateUpdateCookbookPage.xaml#L161)
- [Cookbook Details Page](https://github.com/unoplatform/uno.chefs/blob/main/src/Chefs/Views/CookbookDetailPage.xaml#L123)
- [Favorite Recipes Page](https://github.com/unoplatform/uno.chefs/blob/main/src/Chefs/Views/FavoriteRecipesPage.xaml#L331)
- [Live Cooking Page](https://github.com/unoplatform/uno.chefs/blob/main/src/Chefs/Views/LiveCookingPage.xaml#L34)
- [Chart Control](https://github.com/unoplatform/uno.chefs/blob/main/src/Chefs/Views/Controls/ChartControl.xaml#L36)

### Documentation

- [ResponsiveExtension documentation](https://platform.uno/docs/articles/external/uno.toolkit.ui/doc/helpers/responsive-extension.html)