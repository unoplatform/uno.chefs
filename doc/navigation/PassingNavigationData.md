---
uid: Uno.Recipes.PassingNavigationData
---

# Passing Navigation Data

## Problem

Passing data between pages or view models is a common requirement in modern applications. Without a consistent and reliable method for passing data during navigation, maintaining state and user context can become challenging and error-prone.

## Solution

The `INavigator` interface provided by `Uno.Extensions` allows robust mechanisms to navigate back with results and pass data when navigating.

### Passing Data Between ViewModels

In the Chefs app, when applying filters on the **SearchPage**, we use the `NavigateBackWithResultAsync` method from the `INavigator` interface.

```csharp
public async ValueTask ApplySearchFilter(SearchFilter filter) =>
    await _navigator.NavigateBackWithResultAsync(this, data: filter);
```

### Applying filters on the Search page

The `FilterModel` handles the updating of a `SearchFilter` entity. When navigating to `FilterModel` from the **SearchPage**, a `SearchFilter` entity can be passed and used for updating an existing `SearchFilter`.

The code snippet below shows a `Button` that opens the `FiltersPage`. The key part is `uen:Navigation.Data="{Binding Filter.Value, Mode=TwoWay}"`, which passes the current `Filter` value from the `SearchModel` to the `FilterModel`. Because it's a `TwoWay` binding, any changes made in the `FiltersPage` are reflected back in the original model after navigation completes.

```xml
<Button x:Name="FiltersButton"
        uen:Navigation.Data="{Binding Filter.Value, Mode=TwoWay}"
        uen:Navigation.Request="!Filter"
        Content="Filters"
        CornerRadius="20"
        Foreground="{ThemeResource PrimaryBrush}"
        Style="{StaticResource TextButtonStyle}">
    <ut:ControlExtensions.Icon>
        <PathIcon Data="{StaticResource Icon_Tune}"
                    Foreground="{ThemeResource PrimaryBrush}" />
    </ut:ControlExtensions.Icon>
</Button>
```

The updated `SearchFilter` with the new filter choices is returned to the **SearchPage** via `NavigateBackWithResultAsync`.

To enable data passing, make sure the navigation routes are properly configured in `App.xaml.host.cs`:

In the `RegisterRoutes` method, within the view registration block, configure a `DataViewMap` like this:

```csharp
new DataViewMap<SearchPage, SearchModel, SearchFilter>()
```

Then, in the route registration block, configure the `RouteMap`s like this:

```csharp
new RouteMap("Search", View: views.FindByViewModel<SearchModel>()),
new RouteMap("Filter", View: views.FindByViewModel<FilterModel>())
```

## Source Code

- [NavigateBackWithResultAsync](https://github.com/unoplatform/uno.chefs/blob/139edc9eab65b322e219efb7572583551c40ad32/Chefs/Presentation/FilterModel.cs#L21-L22)
- [App.xaml.host.cs](https://github.com/unoplatform/uno.chefs/blob/04a93886dd0b530386997179b80453a59e832fbe/Chefs/App.xaml.host.cs#L134)

## Documentation

- [Navigation - How to request a Value](xref:Uno.Extensions.Navigation.HowToSelectValue)