---
uid: Uno.Recipes.EntityCreation
---

# How to return data through Navigation for entity creation

## Problem

In a situation where we want to show the details of an entity after editing/creating it in a separate page, handling the data and updating the view accordingly can be difficult.

## Solution

[Uno Extensions Navigation](xref:Uno.Extensions.Navigation.Overview) provides `NavigateForResultAsync()`, an extension method that can handle entity creation on another page.

### NavigateForResultAsync

Let's take a look at an example where we would create a recipe on another page and want to display its details in the same method:

```csharp
// Navigate to the recipe creation page, and await the response
var response = await _navigator.NavigateForResultAsync<Recipe>(this);

var createdRecipe = await response.Result;

Recipe = createdRecipe; // the Recipe we now display on this page is the created one

...
```

For this to work, we need to register the routes in the root App.cs file correctly, with a `ResultData` and `FindByResultData()`:

```csharp
views.Register(
    /* other ViewMaps */,
    new ViewMap<RecipeCreationPage, RecipeCreationViewModel>(ResultData: typeof(Recipe))
);

routes.Register(
    new RouteMap("", View: views.FindByViewModel<ShellViewModel>(), Nested: new RouteMap[]
    {
        /* other RouteMaps */,
        new RouteMap("RecipeCreation", View: views.FindByResultData<Recipe>())
    })
);
```

### NavigateBackWithResultAsync

In Chefs, the Cookbook editing/creation page is opened as a dialog. When we're finished editing/creating a Cookbook (adding recipes to it), we want to be able to submit the new cookbook and navigate back, where we will see the details of the one we just modified/created.

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

        ...

        await _navigator.NavigateBackWithResultAsync(this, data: response); // response is the Cookbook that we just edited/created
    }
}
```

We can see in the code above that once the Cookbook is edited/created in the Cookbook service, we navigate back to the previous page with the navigation data being the new Cookbook.

## Source Code

- [Cookbook NavigateBackWithResultAsync](https://github.com/unoplatform/uno.chefs/blob/9541aa5e0fbbc1c1598dfce4153a9a7fc4e95ccd/src/Chefs/Presentation/CreateUpdateCookbookModel.cs#L72)

## Documentation

- [Uno Extensions Navigation: How to request a value](xref:Uno.Extensions.Navigation.HowToSelectValue)
