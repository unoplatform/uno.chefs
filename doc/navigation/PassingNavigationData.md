---
uid: Uno.Recipes.PassingNavigationData
---

# How to Pass Data Across Navigation Requests

## Problem

Passing data between pages or view models is a common requirement in modern applications. Without a consistent and reliable method for passing data during navigation, maintaining state and user context can become challenging and error-prone.

## Solution

The `INavigator` interface provided by `Uno.Extensions` allows robust mechanisms to navigate back with results and pass data when navigating.

### Passing Data Between ViewModels

In the Chefs app, when creating/updating a Cookbook, we use the `NavigateBackWithResultAsync` method from the `INavigator` interface.

[!code-csharp[](../../Chefs/Presentation/CreateUpdateCookbookModel.cs#L65-L87)]

### Creating or Updating a Cookbook

The `CreateUpdateCookbookModel` handles the creation or updating of a `Cookbook` entity. When navigating to `CreateUpdateCookbookModel`, a `Cookbook` entity can be passed and used for updating an existing `Cookbook`. If no `Cookbook` is passed as a navigation parameter, we can assume that we will be creating a new `Cookbook`.

To return the created or updated Cookbook to the previous view:

```csharp
// Passing the updated or newly created Cookbook back
await _navigator.NavigateBackWithResultAsync(this, data: response);
```

To enable data passing, make sure the navigation routes are properly configured in `App.xaml.cs`:

In the `RegisterRoutes` method, within the view registration block, configure a `DataViewMap` like this:

[!code-csharp[](../../Chefs/App.xaml.cs#L172)]

Then, in the route registration block, configure the `RouteMap`s like this:

[!code-csharp[](../../Chefs/App.xaml.cs#L205-L206)]

## Source Code

Chefs app

- [Create Update Cookbook Model](https://github.com/unoplatform/uno.chefs/blob/139edc9eab65b322e219efb7572583551c40ad32/Chefs/Presentation/CreateUpdateCookbookModel.cs#L14)
- [App.xaml.cs](https://github.com/unoplatform/uno.chefs/blob/139edc9eab65b322e219efb7572583551c40ad32/Chefs/App.xaml.cs#L172)

## Documentation

- [Navigation - How to request a Value](xref:Uno.Extensions.Navigation.HowToSelectValue)