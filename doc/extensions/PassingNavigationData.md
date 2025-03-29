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

```csharp
public async ValueTask Submit(CancellationToken ct)
	{
		var selectedRecipes = await SelectedRecipes;
		var cookbook = await Cookbook;

		if (selectedRecipes is { Count: > 0 } && cookbook is not null && cookbook.Name.HasValueTrimmed())
		{
			var response = IsCreate
				? await _cookbookService.Create(cookbook.Name!, selectedRecipes.ToImmutableList(), ct)
				: await _cookbookService.Update(cookbook, selectedRecipes, ct);

			if (IsCreate)
			{
				await _cookbookService.Save(response!, ct);
			}

			await _navigator.NavigateBackWithResultAsync(this, data: response);
		}
		else
		{
			await _navigator.ShowDialog(this, new DialogInfo("Error", "Please write a cookbook name and select one recipe."), ct);
		}
	}
```

### Creating or Updating a Cookbook

The `CreateUpdateCookbookModel` handles the creation or updating of a `Cookbook` entity. When navigating to `CreateUpdateCookbookModel`, a `Cookbook` entity can be passed and used for updating an existing `Cookbook`. If no `Cookbook` is passed as a navigation parameter, we can assume that we will be creating a new `Cookbook`.

To return the created or updated Cookbook to the previous view:

```csharp
// Passing the updated or newly created Cookbook back
await _navigator.NavigateBackWithResultAsync(this, data: response);
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

- [Create Update Cookbook Model](https://github.com/unoplatform/uno.chefs/blob/92105f64923058b9ace3897bbea17cdb3b354fe9/src/Chefs/Presentation/CreateUpdateCookbookModel.cs#L11)
- [App](https://github.com/unoplatform/uno.chefs/blob/92105f64923058b9ace3897bbea17cdb3b354fe9/src/Chefs/App.cs#L100)

## Documentation

- [Navigation- How to select a Value](xref:Uno.Extensions.Navigation.HowToSelectValue)
- [Navigation- How to request a Value](xref:Uno.Extensions.Navigation.HowToSelectValue)