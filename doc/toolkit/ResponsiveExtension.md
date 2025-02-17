---
uid: Uno.Recipes.ResponsiveExtension
---

# How to adapt properties based on screen size in XAML

## Problem

XAML currently lacks a simple and flexible approach for creating responsive layouts. While options like `VisualStates` with `AdaptiveTriggers` exist, they can become cumbersome even in simple scenarios that do not involve significant UI structure changes based on screen size.

## Solution

The `ResponsiveExtension` class is a markup extension that enables the customization of `UIElement` properties based on screen size.

![ResponsiveView Scenario](../assets/responsiveview-sample.gif)

`ResponsiveExtension` is conceived as a lightweight approach to responsiveness. For more complex scenarios, where the UI structure changes significantly based on screen size, consider using the [`ResponsiveView`](xref:Uno.Recipes.ResponsiveView) control.

```xml
<utu:AutoLayout Padding="{utu:Responsive Narrow='16,24', Wide='40,40,40,24'}" />
                ...
</utu:AutoLayout>
```

![Responsive Extension Animation"](../assets/responsiveextension-animated.gif)

## Source Code

- [Main Page (1)](https://github.com/unoplatform/uno.chefs/blob/19ace5c583ef4ef55f019589dd1eb07e43000de9/src/Chefs/Views/MainPage.xaml#L37-L38)
- [Main Page (2)](https://github.com/unoplatform/uno.chefs/blob/19ace5c583ef4ef55f019589dd1eb07e43000de9/src/Chefs/Views/MainPage.xaml#L65-L66)
- [Search Page](https://github.com/unoplatform/uno.chefs/blob/c39edbc737dfd899b31cb3ba24d017c9e8351861/src/Chefs/Views/SearchPage.xaml#L148)
- [Home Page](https://github.com/unoplatform/uno.chefs/blob/c39edbc737dfd899b31cb3ba24d017c9e8351861/src/Chefs/Views/HomePage.xaml#L290)
- [Recipe Details Page](https://github.com/unoplatform/uno.chefs/blob/c39edbc737dfd899b31cb3ba24d017c9e8351861/src/Chefs/Views/RecipeDetailsPage.xaml#L24)
- [Create/Update Cookbook Page](https://github.com/unoplatform/uno.chefs/blob/c39edbc737dfd899b31cb3ba24d017c9e8351861/src/Chefs/Views/CreateUpdateCookbookPage.xaml#L152)
- [Cookbook Details Page](https://github.com/unoplatform/uno.chefs/blob/c39edbc737dfd899b31cb3ba24d017c9e8351861/src/Chefs/Views/CookbookDetailPage.xaml#L114)
- [Favorite Recipes Page](https://github.com/unoplatform/uno.chefs/blob/c39edbc737dfd899b31cb3ba24d017c9e8351861/src/Chefs/Views/FavoriteRecipesPage.xaml#L331)
- [Live Cooking Page](https://github.com/unoplatform/uno.chefs/blob/c39edbc737dfd899b31cb3ba24d017c9e8351861/src/Chefs/Views/LiveCookingPage.xaml#L34)
- [Chart Control](https://github.com/unoplatform/uno.chefs/blob/c39edbc737dfd899b31cb3ba24d017c9e8351861/src/Chefs/Views/Controls/ChartControl.xaml#L36)

## Documentation

- [ResponsiveExtension documentation](xref:Toolkit.Helpers.ResponsiveExtension)
