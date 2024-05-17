---
uid: Uno.Recipes.AlternateContentControlExtension
---

# How to switch ToggleButton Content when toggled

## Problem

The base `ToggleButton` control does not provide a way to switch its content when toggled. This is a common pattern in modern UIs, where a `ToggleButton` can be used to switch between two states, and the content should change accordingly.

## Solution

The [Uno Themes library](xref:Uno.Themes.Overview) provides a set of attached properties grouped under the `ControlExtensions` class. One of these attached properties is the `AlternateContent` property. The [Uno Material library](Uno.Themes.Material.GetStarted) provides custom styles for the `ToggleButton` that support the `AlternateContent` property.

### ToggleButton AlternateContent

Given the following XAML:

```xml
xmlns:ut="using:Uno.Themes"

<ToggleButton Style="{StaticResource IconToggleButtonStyle}">
    <ToggleButton.Content>
        <FontIcon Style="{StaticResource FontAwesomeRegularFontIconStyle}"
                  Glyph="{StaticResource Icon_Favorite}"
                  Foreground="{ThemeResource OnSurfaceBrush}" />
    </ToggleButton.Content>
    <ut:ControlExtensions.AlternateContent>
        <FontIcon Style="{StaticResource FontAwesomeSolidFontIconStyle}"
                  Glyph="{StaticResource Icon_Favorite}"
                  Foreground="{ThemeResource PrimaryBrush}" />
    </ut:ControlExtensions.AlternateContent>
</ToggleButton>
```

![ToggleButton with AlternateContent](../assets/toggle-alternate-content.gif)

## Source Code

Chefs app

- [Home Page](https://github.com/unoplatform/uno.chefs/blob/08d57612e22fd6796e9f0ee7a8a48ba252e7440a/src/Chefs/Views/HomePage.xaml#L57-L61)
- [Recipe Details Page](https://github.com/unoplatform/uno.chefs/blob/08d57612e22fd6796e9f0ee7a8a48ba252e7440a/src/Chefs/Views/RecipeDetailsPage.xaml#L353-L365)

## Documentation

- [AlternateContent ControlExtensions documentation](xref:Uno.Themes.Control.Extensions#alternate-content)
