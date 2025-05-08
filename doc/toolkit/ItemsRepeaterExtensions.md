---
uid: Uno.Recipes.ItemsRepeaterExtensions
---

# Enabling Selection in ItemsRepeater Control

## Problem

While the [`ItemsRepeater` control](https://learn.microsoft.com/en-us/windows/apps/design/controls/items-repeater) provides a flexible system for creating custom layouts, it lacks built-in support for common selection policies. For example, the `ListView` control has built-in support for single and multiple selection modes, but the ItemsRepeater control does not.

## Solution

The `ItemsRepeater` extensions library provides attached properties that enable common selection policies on the `ItemsRepeater` control. This allows you to easily implement single, multiple, and single-or-none selection modes on the `ItemsRepeater` control.

```xml
<muxc:ItemsRepeater ItemsSource="{Binding Times}"
                    utu:ItemsRepeaterExtensions.SelectedItem="{Binding Filter.Time, Mode=TwoWay}"
                    utu:ItemsRepeaterExtensions.SelectionMode="SingleOrNone">
```

![ItemsRepeaterExtensions Single Selection ExampleItemsRepeaterExtensions Single Selection Example](../assets/itemsrepeater-extensions-single.gif)

## Source Code

- [Filters Page (1)](https://github.com/unoplatform/uno.chefs/blob/139edc9eab65b322e219efb7572583551c40ad32/Chefs/Views/FiltersPage.xaml#L52)
- [Filters Page (2)](https://github.com/unoplatform/uno.chefs/blob/139edc9eab65b322e219efb7572583551c40ad32/Chefs/Views/FiltersPage.xaml#L71)
- [Filters Page (3)](https://github.com/unoplatform/uno.chefs/blob/139edc9eab65b322e219efb7572583551c40ad32/Chefs/Views/FiltersPage.xaml#L99)
- [Filters Page (4)](https://github.com/unoplatform/uno.chefs/blob/139edc9eab65b322e219efb7572583551c40ad32/Chefs/Views/FiltersPage.xaml#L118)

## Documentation

- [ItemsRepeater Extensions documentation](xref:Toolkit.Helpers.ItemRepeaterExtensions)
