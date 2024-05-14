---
uid: Uno.Recipes.MvuxNavigation
---

# How-To: Navigate in MVUX Model

## Problem

Navigation implementations vary between platforms, and it can be time-consuming to create and manage all the different navigation methods with each their own destination and data.

## Solution

Uno Navigation simplifies development by abstracting away platform-specific navigation implementations and promotes code reusability, which allows you to focus on building features rather than managing navigation intricacies.

Uno Navigation in C# `INavigator` provides many methods and extensions to navigate between view models, some with and without data. It is also possible to [navigate between pages](xref:Uno.Extensions.Navigation.HowToNavigateInCode#1-navigating-to-a-new-page), but that won't be covered here as page navigation cannot carry data. Let's take a look into each of the navigation methods that are used in Chefs.

### 1. NavigateViewModelAsync

This method navigates to a route that matches the specified view model type. Let's say we want to navigate to the search page and display popular recipes. To do so, we can use `NavigateViewModelAsync` to navigate to the _SearchModel_ and we can add a `SearchFilter` as data to specifiy that the recipes should be organized by popularity.

```csharp
public async ValueTask SearchPopular(CancellationToken ct) =>
	await _navigator.NavigateViewModelAsync<SearchModel>(this, data: new SearchFilter(OrganizeCategory.Popular, null, null, null, null));
```

### 2. NavigateDataAsync

This method navigates to a route that is registered for the specified data type.

```csharp
public static Task<NavigationResponse?> ShowDialog(this INavigator navigator, object sender, DialogInfo dialogInfo, CancellationToken ct)
{
    return navigator.NavigateDataAsync(sender, new DialogInfo(dialogInfo.Title, dialogInfo.Content), cancellation: ct);
}
```

Since we defined that the `GenericDialogModel` is registered to a _DialogInfo_ _DataMap_, it will choose the "Dialog" route:

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

This method navigates back to the previous frame on the back-stack, eliminating the need to remake the view model.

```csharp
private async Task NavigateBack()
{
    await _navigator.NavigateBackAsync(this);
}
```

### 4. NavigateBackWithResultAsync

This method navigates back to the previous view model with data. Let's say the previous page the user was on was the search page and it was displaying recipes with a certain filter. What if the user navigates to the filter page, changes the search filters, and once submitted is navigated back to the search page? We could use `NavigateViewModelAsync` but that would mean remaking the `SearchModel`. That's why we should use `NavigateBackWithResultAsync` and pass the new filter as data.

```csharp
public async ValueTask Search(SearchFilter filter, CancellationToken ct) =>
	await _navigator.NavigateBackWithResultAsync(this, data: filter, cancellation: ct);
```

### 5. NavigateRouteAsync

This method navigates to a registered route without having to specify a view model. It may seem simple but this allows us to do some pretty neat dynamic routing, here's an example:

```csharp
public async ValueTask LiveCooking(IImmutableList<Step> steps, CancellationToken ct)
{
    var route = _navigator?.Route?.Base switch
    {
        "RecipeDetails" => "LiveCooking",
        "SearchRecipeDetails" => "SearchLiveCooking",
        "FavoriteRecipeDetails" => "FavoriteLiveCooking",
        "CookbookRecipeDetails" => "CookbookLiveCooking",
        _ => throw new InvalidOperationException("Navigating from unknown route")
    };

    await _navigator.NavigateRouteAsync(this, route, data: new LiveCookingParameter(Recipe, steps), cancellation: ct);
}
```

When the _LiveCooking_ `ValueTask` is triggered, we check where the navigation was last. If the navigation is coming from the _FavoriteRecipeDetails_ base route, we set the next route to be _FavoriteLiveCooking_ and pass this route to `NavigateRouteAsync`. We can also pass data with this method, as seen here with _LiveCookingParameter_. It can accept _LiveCookingParameter_ as data because of our registered routing. The `ViewMap` registers the _LiveCookingModel_ to the _LiveCookingParameter_ `DataMap` and the `RouteMap` registers the _FavoriteLiveCooking_ route to the _LiveCookingModel_ view:

```csharp
private static void RegisterRoutes(IViewRegistry views, IRouteRegistry routes)
{
    views.Register(
        /* other routes */,
        new ViewMap<LiveCookingPage, LiveCookingModel>(Data: new DataMap<LiveCookingParameter>())
    );

    routes.Register(
        new RouteMap("", View: views.FindByViewModel<ShellModel>(),
            Nested: new RouteMap[]
            {
                /* other routes */,
                new RouteMap("Main", View: views.FindByViewModel<MainModel>(), Nested: new RouteMap[]
                {
                    /* other routes*/,
                    new RouteMap("LiveCooking", View: views.FindByViewModel<LiveCookingModel>(), DependsOn: "RecipeDetails"),
                    new RouteMap("SearchLiveCooking", View: views.FindByViewModel<LiveCookingModel>(), DependsOn: "SearchRecipeDetails"),
                    new RouteMap("FavoriteLiveCooking", View: views.FindByViewModel<LiveCookingModel>(), DependsOn: "FavoriteRecipeDetails"),
                    new RouteMap("CookbookLiveCooking", View: views.FindByViewModel<LiveCookingModel>(), DependsOn: "CookbookRecipeDetails")
                })
            }
        )
    );
}
```

### 6. NavigateRouteForResultAsync

This method navigates to a registered route and returns an awaitable response.

```csharp
public static async ValueTask NavigateToProfile(this INavigator navigator, object sender, User? profile = null)
{
    var response = await navigator.NavigateRouteForResultAsync<Recipe?>(sender, "Profile", qualifier: Qualifiers.Dialog, data: profile);
    var result = await response!.Result;

    ...
}
```

## Source Code

- [NavigateViewModelAsync](https://github.com/unoplatform/uno.chefs/blob/f7ccfcc2d47d7d45e2ae34a1a251d8c95311c309/src/Chefs/Presentation/SearchModel.cs#L81-L82)
- [NavigateDataAsync](https://github.com/unoplatform/uno.chefs/blob/f7ccfcc2d47d7d45e2ae34a1a251d8c95311c309/src/Chefs/Presentation/Extensions/INavigatorExtensions.cs#L36-L39)
- [NavigateBackAsync](https://github.com/unoplatform/uno.chefs/blob/f7ccfcc2d47d7d45e2ae34a1a251d8c95311c309/src/Chefs/Presentation/NotificationsModel.cs#L32-L35)
- [NavigateBackWithResultAsync](https://github.com/unoplatform/uno.chefs/blob/f7ccfcc2d47d7d45e2ae34a1a251d8c95311c309/src/Chefs/Presentation/FilterModel.cs#L25-L26)
- [NavigateRouteAsync](https://github.com/unoplatform/uno.chefs/blob/f7ccfcc2d47d7d45e2ae34a1a251d8c95311c309/src/Chefs/Presentation/RecipeDetailsModel.cs#L41-L53)
- [NavigateRouteForResultAsync](https://github.com/unoplatform/uno.chefs/blob/f7ccfcc2d47d7d45e2ae34a1a251d8c95311c309/src/Chefs/Presentation/Extensions/INavigatorExtensions.cs#L20)

## Documentation

- [How-To: Navigate in Code](xref:Uno.Extensions.Navigation.HowToNavigateInCode)
