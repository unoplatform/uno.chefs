---
uid: Uno.Recipes.CommandExtensions
---

# How to invoke an ICommand in XAML for common scenarios without a Command property

## Problem

Current controls may not always provide a `Command` property to bind to an `ICommand` in XAML. This can make it difficult to invoke commands in response to user interactions such as when a `TextBox`/`PasswordBox` enter key is pressed, a `ListViewBase`'s `ItemClick` is fired, `NavigationView.ItemInvoked`, or an `ItemsRepeater` item is tapped.

## Solution

The `CommandExtensions` class in the Uno Toolkit provides `Command`/`CommandParameter` attached properties for common scenarios.

### PasswordBox Enter Key

[!code-xml[](../../Chefs/Views/LoginPage.xaml#L39-L41)]

> [!TIP]
> Usage on `TextBox`/`PasswordBox` will also cause the keyboard dismiss on enter. Similar to the `InputExtensions.AutoDismiss` behavior, covered in the [InputExtensions](xref:Uno.Recipes.InputExtensions) recipe.

### ItemsRepeater Item Tapped

[!code-xml[](../../Chefs/Views/HomePage.xaml#L142-L143)]

## Source Code

Chefs app

- [Login Page (PasswordBox)](https://github.com/unoplatform/uno.chefs/blob/139edc9eab65b322e219efb7572583551c40ad32/Chefs/Views/LoginPage.xaml#L41)
- [Home Page (ItemsRepeater)](https://github.com/unoplatform/uno.chefs/blob/139edc9eab65b322e219efb7572583551c40ad32/Chefs/Views/HomePage.xaml#L143)
- [Search Page (TextBox)](https://github.com/unoplatform/uno.chefs/blob/139edc9eab65b322e219efb7572583551c40ad32/Chefs/Views/SearchPage.xaml#L114)

## Documentation

- [CommandExtensions documentation](xref:Toolkit.Helpers.CommandExtensions)
