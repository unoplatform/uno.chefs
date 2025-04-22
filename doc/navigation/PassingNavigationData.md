---
uid: Uno.Recipes.PassingNavigationData
---

# How to Pass Data Across Navigation Requests

## Problem

Passing data between pages or view models is a common requirement in modern applications. Without a consistent and reliable method for passing data during navigation, maintaining state and user context can become challenging and error-prone.

## Solution

The `INavigator` interface provided by `Uno.Extensions` allows robust mechanisms to navigate back with results and pass data when navigating.

### Passing Data Between ViewModels

In the Chefs app, when applying filters on the **SearchPage**, we use the `NavigateBackWithResultAsync` method from the `INavigator` interface.

[!code-csharp[](../../Chefs/Presentation/FilterModel.cs#L21-L22)]

### Applying filters on the Search page

The `FilterModel` handles the updating of a `SearchFilter` entity. When navigating to `FilterModel` from the **SearchPage**, a `SearchFilter` entity can be passed and used for updating an existing `SearchFilter`.

The code snippet below shows a `Button` that opens the `FiltersPage`. The key part is `uen:Navigation.Data="{Binding Filter.Value, Mode=TwoWay}"`, which passes the current `Filter` value from the `SearchModel` to the `FilterModel`. Because it's a `TwoWay` binding, any changes made in the `FiltersPage` are reflected back in the original model after navigation completes.

[!code-xml[](../../Chefs/Views/SearchPage.xaml#L144-L147)]

To return the updated `SearchFilter` with the new filters choices to the **SearchPage**:

[!code-csharp[](../../Chefs/Presentation/FilterModel.cs#L22)]

To enable data passing, make sure the navigation routes are properly configured in `App.xaml.cs`:

In the `RegisterRoutes` method, within the view registration block, configure a `DataViewMap` like this:

[!code-csharp[](../../Chefs/App.xaml.cs#L174)]

Then, in the route registration block, configure the `RouteMap`s like this:

[!code-csharp[](../../Chefs/App.xaml.cs#L231)]

## Source Code

Chefs app

- [NavigateBackWithResultAsync](https://github.com/unoplatform/uno.chefs/blob/4c94f3ec749e1295470950018cd960f74a109ca3/Chefs/App.xaml.cs#L174)
- [App.xaml.cs](https://github.com/unoplatform/uno.chefs/blob/419ddbc6730da4f3742e74ecc9a780dc851b6d5a/Chefs/App.xaml.cs#L174)

## Documentation

- [Navigation - How to request a Value](xref:Uno.Extensions.Navigation.HowToSelectValue)