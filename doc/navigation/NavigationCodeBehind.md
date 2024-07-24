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

```csharp
public async ValueTask SearchPopular() =>
    await _navigator.NavigateViewModelAsync<SearchModel>(this, data: new SearchFilter(FilterGroup: FilterGroup.Popular));
```

### 2. NavigateDataAsync

This method navigates to a route that is registered for the specified data type. In the root App.xaml.cs, since we defined that the `GenericDialogModel` is registered to a `DialogInfo` through the `DataMap`, it will choose the "Dialog" route.

```csharp
public static Task<NavigationResponse?> ShowDialog(this INavigator navigator, object sender, DialogInfo dialogInfo, CancellationToken ct)
{
    return navigator.NavigateDataAsync(sender, new DialogInfo(dialogInfo.Title, dialogInfo.Content), cancellation: ct);
}
```

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

```csharp
public async ValueTask ApplySearchFilter(SearchFilter filter) =>
    await _navigator.NavigateBackWithResultAsync(this, data: filter);
```

### 5. NavigateRouteAsync

This method navigates to a registered route, if it exists, without having to specify a view model.

```csharp
public async ValueTask LiveCooking(IImmutableList<Step> steps, CancellationToken ct)
{
    await _navigator.NavigateRouteAsync(this, "LiveCooking", data: new LiveCookingParameter(Recipe, steps), cancellation: ct);
}
```

## Source Code

- [NavigateViewModelAsync](https://github.com/unoplatform/uno.chefs/blob/5b7bf94fca19eee93de38fc81e08aa1f40804c47/src/Chefs/Presentation/SearchModel.cs#L71-L72)
- [NavigateDataAsync](https://github.com/unoplatform/uno.chefs/blob/5b7bf94fca19eee93de38fc81e08aa1f40804c47/src/Chefs/Presentation/Extensions/INavigatorExtensions.cs#L13-L16)
- [NavigateBackAsync](https://github.com/unoplatform/uno.chefs/blob/5b7bf94fca19eee93de38fc81e08aa1f40804c47/src/Chefs/Presentation/NotificationsModel.cs#L32-L35)
- [NavigateBackWithResultAsync](https://github.com/unoplatform/uno.chefs/blob/5b7bf94fca19eee93de38fc81e08aa1f40804c47/src/Chefs/Presentation/FilterModel.cs#L20-L21)
- [NavigateRouteAsync](https://github.com/unoplatform/uno.chefs/blob/f7ccfcc2d47d7d45e2ae34a1a251d8c95311c309/src/Chefs/Presentation/RecipeDetailsModel.cs#L41-L53)

## Documentation

- [How-To: Navigate in Code](xref:Uno.Extensions.Navigation.HowToNavigateInCode)
