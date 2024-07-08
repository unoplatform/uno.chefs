---
uid: Uno.Recipes.NavigationOverview
---

## Problem

Navigation implementations vary between platforms, and it can be time-consuming to create and manage all the different navigation methods with each their own destination and data.

## Solution

Uno Extensions Navigation simplifies development by abstracting away platform-specific navigation implementations and promotes code reusability, which allows you to focus on building features rather than managing navigation intricacies.

## General Usage

It's an easy setup, you only need to include the corresponding [Uno Feature](xref:Uno.Features.Uno.Sdk#uno-platform-features) to your project file:

```xml
<UnoFeatures>
    Navigation;
</UnoFeatures>
```

You can then start using Uno Extensions Navigation across your app. Here are some more concrete examples from Chefs using the Navigation extension:

- [Navigation with Xaml RecipeBook](xref:Uno.Recipes.XamlNavigation)
- [Navigation using Code Behind RecipeBook](xref:Uno.Recipes.NavigationCodeBehind)

## Documentation

- [Uno Extensions Navigation](xref:Uno.Extensions.Navigation.Overview)