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
<Page ...
      xmlns:ut="using:Uno.Themes">

    <Button Style="{StaticResource FabStyle}">
        <ut:ControlExtensions.Icon>
            <SymbolIcon Symbol="Edit" />
        </ut:ControlExtensions.Icon>
    </Button>

</Page>
```

![FAB with Icon](../assets/fab-icon.png)

### TextBox/PasswordBox Icon

```xml
<Page ...
      xmlns:ut="using:Uno.Themes">

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

</Page>
```

![Login Controls with Icon](../assets/login-icon.png)

## Source Code

Chefs app

- [Login Page (TextBox)](https://github.com/unoplatform/uno.chefs/blob/19ace5c583ef4ef55f019589dd1eb07e43000de9/src/Chefs/Views/LoginPage.xaml#L35-L37)
- [Login Page (PasswordBox)](https://github.com/unoplatform/uno.chefs/blob/19ace5c583ef4ef55f019589dd1eb07e43000de9/src/Chefs/Views/LoginPage.xaml#L60-L62)
- [Settings Page (TextBox)](https://github.com/unoplatform/uno.chefs/blob/19ace5c583ef4ef55f019589dd1eb07e43000de9/src/Chefs/Views/SettingsPage.xaml#L49-L52)
- [Favorite Recipes Page (Button)](https://github.com/unoplatform/uno.chefs/blob/19ace5c583ef4ef55f019589dd1eb07e43000de9/src/Chefs/Views/FavoriteRecipesPage.xaml#L57-L61)
- [Profile Page (Button)](https://github.com/unoplatform/uno.chefs/blob/19ace5c583ef4ef55f019589dd1eb07e43000de9/src/Chefs/Views/ProfilePage.xaml#L49-L52)
- [Live Cooking Page (Button)](https://github.com/unoplatform/uno.chefs/blob/19ace5c583ef4ef55f019589dd1eb07e43000de9/src/Chefs/Views/LiveCookingPage.xaml#L253-L255)

## Documentation

- [Icon ControlExtensions documentation](xref:Uno.Themes.Control.Extensions#icon)
