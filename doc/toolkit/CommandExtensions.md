---
uid: Uno.Recipes.CommandExtensions
---

# How to invoke an ICommand in XAML for common scenarios without a Command property

## Problem

Current controls may not always provide a `Command` property to bind to an `ICommand` in XAML. This can make it difficult to invoke commands in response to user interactions such as when a `TextBox`/`PasswordBox` enter key is pressed, a `ListViewBase`'s `ItemClick` is fired, `NavigationView.ItemInvoked`, or an `ItemsRepeater` item is tapped.

## Solution

The `CommandExtensions` class in the Uno Toolkit provides `Command`/`CommandParameter` attached properties for common scenarios.

### PasswordBox Enter Key

```xml
...
<PasswordBox PlaceholderText="Password"
             utu:CommandExtensions.Command="{Binding Login}"
             ... />
...
<Button Content="Login"
        Command="{Binding Login}" />
...
```

> [!TIP]
> Uasge on `TextBox`/`PasswordBox` will also cause the keyboard dismiss on enter. Similar to the `InputExtensions.AutoDismiss` behavior, covered in the [InputExtensions](xref:Uno.Recipes.InputExtensions) recipe.

### ItemsRepeater Item Tapped

```xml
<muxc:ItemsRepeater utu:CommandExtensions.Command="{Binding SelectRecipe}"
                    ItemTemplate="{StaticResource RecipeTemplate}"
                    ItemsSource="{Binding Recipes}" />
```

## Source Code

Chefs app

- [Login Page](https://github.com/unoplatform/uno.chefs/blob/c39edbc737dfd899b31cb3ba24d017c9e8351861/src/Chefs/Views/LoginPage.xaml#L74)
- [Home Page](https://github.com/unoplatform/uno.chefs/blob/c39edbc737dfd899b31cb3ba24d017c9e8351861/src/Chefs/Views/HomePage.xaml#L155)
- [Search Page](https://github.com/unoplatform/uno.chefs/blob/c39edbc737dfd899b31cb3ba24d017c9e8351861/src/Chefs/Views/SearchPage.xaml#L152)
- [CreateUpdateCookbook Page](https://github.com/unoplatform/uno.chefs/blob/c39edbc737dfd899b31cb3ba24d017c9e8351861/src/Chefs/Views/CreateUpdateCookbookPage.xaml#L149)

## Documentation

- [CommandExtensions documentation](xref:Toolkit.Helpers.CommandExtensions)