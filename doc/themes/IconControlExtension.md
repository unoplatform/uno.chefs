---
uid: Uno.Recipes.IconControlExtension
---

# How to add custom icons to supported controls

## Problem

Some controls do not offer support for custom icons, and would normally require a custom style to edit the control's template to support `IconElement`s.

## Solution

The Uno Themes library provides a set of attached properties grouped under the `ControlExtensions` class. One of these attached properties is the `Icon` property, which allows you to add custom icons to certain controls. Specific styles are provided for each control to support the `Icon` property.

### Button Icon

Given the following XAML:

```xml
xmlns:ut="using:Uno.Themes"

<Button Style="{StaticResource FabStyle}">
    <ut:ControlExtensions.Icon>
        <SymbolIcon Symbol="Edit" />
    </ut:ControlExtensions.Icon>
</Button>
```

![FAB with Icon](../assets/fab-icon.png)

### TextBox/PasswordBox Icon

```xml
xmlns:ut="using:Uno.Themes"

<!-- Omitted code -->

<TextBox PlaceholderText="Username"
         Style="{StaticResource OutlinedTextBoxStyle}">
    <ut:ControlExtensions.Icon>
        <FontIcon Glyph="{StaticResource Icon_Person_Outline}" />
    </ut:ControlExtensions.Icon>
</TextBox>
<PasswordBox PlaceholderText="Password"
             Style="{StaticResource OutlinedPasswordBoxStyle}">
    <ut:ControlExtensions.Icon>
        <FontIcon Glyph="{StaticResource Icon_Lock}" />
    </ut:ControlExtensions.Icon>
</PasswordBox>
```

![Login Controls with Icon](../assets/login-icon.png)

## Source Code

Chefs app

- [Login Page (1)](https://github.com/unoplatform/uno.chefs/blob/08d57612e22fd6796e9f0ee7a8a48ba252e7440a/src/Chefs/Views/LoginPage.xaml#L67-L69)
- [Login Page (2)](https://github.com/unoplatform/uno.chefs/blob/08d57612e22fd6796e9f0ee7a8a48ba252e7440a/src/Chefs/Views/LoginPage.xaml#L79-L81)
- [Settings Page](https://github.com/unoplatform/uno.chefs/blob/08d57612e22fd6796e9f0ee7a8a48ba252e7440a/src/Chefs/Views/SettingsPage.xaml#L53-L56)
- [Favorite Recipes Page](https://github.com/unoplatform/uno.chefs/blob/08d57612e22fd6796e9f0ee7a8a48ba252e7440a/src/Chefs/Views/FavoriteRecipesPage.xaml#L71-L74)
- [Profile Page](https://github.com/unoplatform/uno.chefs/blob/08d57612e22fd6796e9f0ee7a8a48ba252e7440a/src/Chefs/Views/ProfilePage.xaml#L53-L56)
- [Live Cooking Page](https://github.com/unoplatform/uno.chefs/blob/08d57612e22fd6796e9f0ee7a8a48ba252e7440a/src/Chefs/Views/LiveCookingPage.xaml#L255-L258)

## Documentation

- [Icon ControlExtensions documentation](xref:Uno.Themes.Control.Extensions#icon)
