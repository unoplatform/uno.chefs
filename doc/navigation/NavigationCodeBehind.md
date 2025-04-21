---
uid: Uno.Recipes.NavigationCodeBehind
---

# How to Navigate with Code Behind

## Problem

Navigation logic can often become complex when it has to handle different states and pass data. These situations can lead to decreased readability and reusability.

## Solution

Uno Extensions Navigation lets you easily [invoke navigation](xref:Uno.Extensions.Navigation.HowToNavigateInCode) with `INavigator` through commands defined in the view model, enabling more flexibility and more maintainable code. It is also possible to [navigate between pages](xref:Uno.Extensions.Navigation.HowToNavigateInCode#1-navigating-to-a-new-page).

### 1. NavigateViewModelAsync

This method navigates to a route that matches the specified view model type. Let's say we want to navigate to the search page and display popular recipes. To do so, we can use `NavigateViewModelAsync` to navigate to the _SearchModel_ and we can add a `SearchFilter` as data to specify that the recipes should be organized by popularity.

[!code-csharp[](../../Chefs/Presentation/SearchModel.cs#L49-L50)]

### 2. NavigateDataAsync

This method navigates to a route that is registered for the specified data type. In the root App.xaml.cs, since we defined that the `GenericDialogModel` is registered to a `DialogInfo` through the `DataMap`, it will choose the "Dialog" route.

[!code-csharp[](../../Chefs/Presentation/Extensions/INavigatorExtensions.cs#L13-L16)]

```csharp
private static void RegisterRoutes(IViewRegistry views, IRouteRegistry routes)
{
    views.Register(
        /* other routes */,
        new ViewMap<GenericDialog, GenericDialogModel>(Data: new DataMap<DialogInfo>())
    );
    
    routes.Register(
        /* other routes */,
        new RouteMap("Dialog", View: views.FindByView<GenericDialog>())
    );
}
```

### 3. NavigateBackAsync

This method navigates back to the previous frame on the back-stack.

```csharp
private async Task NavigateBack()
{
    await _navigator.NavigateBackAsync(this);
}
```

### 4. NavigateBackWithResultAsync

This method navigates back to the previous frame on the back-stack with data. Let's say the previous page the user was on was the search page and it was displaying recipes with a certain filter. What if the user navigates to the filter page, changes the search filters, and once submitted is navigated back to the search page? We could use `NavigateViewModelAsync` but that would mean remaking the `SearchModel`. That's why we should use `NavigateBackWithResultAsync` and pass the new filter as data. We can define a `DataViewMap` to register that the route is expecting a SearchFilter as a parameter when being navigated to.

```csharp
private static void RegisterRoutes(IViewRegistry views, IRouteRegistry routes)
{
    views.Register(
        /* other routes */,
        new DataViewMap<SearchPage, SearchModel, SearchFilter>()
    );
}
```

[!code-csharp[](../../Chefs/Presentation/FilterModel.cs#L21-L22)]

### 5. NavigateRouteAsync

This method navigates to a registered route, if it exists, without having to specify a view model.

```csharp
public async ValueTask LiveCooking(IImmutableList<Step> steps, CancellationToken ct)
{
    await _navigator.NavigateRouteAsync(this, "LiveCooking", data: new LiveCookingParameter(Recipe, steps), cancellation: ct);
}
```

## Source Code

- [NavigateViewModelAsync](https://github.com/unoplatform/uno.chefs/blob/139edc9eab65b322e219efb7572583551c40ad32/Chefs/Presentation/SearchModel.cs#L49-L50)
- [NavigateDataAsync](https://github.com/unoplatform/uno.chefs/blob/139edc9eab65b322e219efb7572583551c40ad32/Chefs/Presentation/Extensions/INavigatorExtensions.cs#L13-L16)
- [NavigateBackWithResultAsync](https://github.com/unoplatform/uno.chefs/blob/139edc9eab65b322e219efb7572583551c40ad32/Chefs/Presentation/FilterModel.cs#L21-L22)
- [NavigateRouteAsync]https://github.com/unoplatform/uno.chefs/blob/139edc9eab65b322e219efb7572583551c40ad32/Chefs/Presentation/RecipeDetailsModel.cs#L51-L63)

## Documentation

- [How-To: Navigate in Code](xref:Uno.Extensions.Navigation.HowToNavigateInCode)
