---
uid: Uno.Recipes.PassingNavigationData
---

# How to Pass Data Across Navigation Requests in the Chefs App

## Problem
Passing data between pages or view models is a common requirement in modern applications. Without a consistent and reliable method for passing data during navigation, maintaining state and user context can become challenging and error-prone.

## Solution
The `INavigator` interface provided by `Uno.Extensions` allows robust mechanisms to navigate back with results and pass data when navigating.

### Passing Data Between ViewModels

In the Chefs app, to pass a `SearchFilter` from `FilterModel` back to `SearchModel`, we use the `NavigateBackWithResultAsync` method from the INavigator interface.

```csharp
public async ValueTask Search(SearchFilter filter, CancellationToken ct) =>
    await _navigator.NavigateBackWithResultAsync(this, data: filter, cancellation: ct);
```
In the SearchModel, we handle the navigation results from the FilterModel:
```csharp
public async ValueTask SearchPopular(CancellationToken ct) =>
    await _navigator.NavigateViewModelAsync<SearchModel>(this, data: new SearchFilter(OrganizeCategory.Popular, null, null, null, null));
```

### Creating or Updating a Cookbook
The `CreateUpdateCookbookModel` handles the creation or updating of a Cookbook. When navigating to CreateUpdateCookbookModel, a `Cookbook` entity can be passed and used for creating or updating.

Here’s how the CreateUpdateCookbookModel handles a Cookbook:
```csharp
namespace Chefs.Presentation
{
    public partial class CreateUpdateCookbookModel
    {
        private readonly INavigator _navigator;
        private readonly Cookbook? _cookbook;

        public CreateUpdateCookbookModel(Cookbook? cookbook, INavigator navigator)
        {
            _navigator = navigator;
            _cookbook = cookbook;
        }
    }
}
```

To enable data passing, ensure that the navigation routes are correctly configured in the `App.xaml.cs`:

```csharp
public class App : Application
{
    private static void RegisterRoutes(IViewRegistry views, IRouteRegistry routes)
    {
        views.Register(
            new DataViewMap<CreateUpdateCookbookPage, CreateUpdateCookbookModel, Cookbook>(),
            // Additional view registrations...
        );

        routes.Register(
            new RouteMap("", View: views.FindByViewModel<ShellModel>(),
                Nested: new RouteMap[]
                {
                    new RouteMap("Main", View: views.FindByViewModel<MainModel>(), Nested: new RouteMap[]
                    {
                        new RouteMap("UpdateCookbook", View: views.FindByViewModel<CreateUpdateCookbookModel>(), DependsOn: "FavoriteRecipes"),
                        new RouteMap("CreateCookbook", View: views.FindByViewModel<CreateUpdateCookbookModel>(), DependsOn: "FavoriteRecipes"),
                    }),
                }
            )
        );
    }
}
```

## Source Code

Chefs app

- [Filter Model](https://github.com/unoplatform/uno.chefs/blob/92105f64923058b9ace3897bbea17cdb3b354fe9/src/Chefs/Presentation/FilterModel.cs#L26)
- [Search Model](https://github.com/unoplatform/uno.chefs/blob/92105f64923058b9ace3897bbea17cdb3b354fe9/src/Chefs/Presentation/SearchModel.cs#L82)
- [Create Update Cookbook Model](https://github.com/unoplatform/uno.chefs/blob/92105f64923058b9ace3897bbea17cdb3b354fe9/src/Chefs/Presentation/CreateUpdateCookbookModel.cs#L11)
- [App](https://github.com/unoplatform/uno.chefs/blob/92105f64923058b9ace3897bbea17cdb3b354fe9/src/Chefs/App.cs#L100)

## Documentation

- [Navigation- How to select a Value](xref:Uno.Extensions.Navigation.HowToSelectValue)
- [Navigation- How to request a Value](xref:Uno.Extensions.Navigation.HowToSelectValue)