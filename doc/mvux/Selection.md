---
uid: Uno.Recipes.Selection
---

# Selecting items from a list with MVUX

## Problem

Selecting items from dynamic lists can be tricky, especially when using paginated data. Doing it manually often adds extra code and makes things more complicated.

## Solution

`Uno.Extensions.MVUX` makes item selection easier by offering a built-in `Selection` method. This lets you manage the selected items with less code and better structure.

### Using Selection

As an example, in Chefs it is possible to create and edit Cookbooks that are a list of recipes. On the **Favorites** page, under the **My Cookbooks** tab, there is a list of existing Cookbooks and the recipes associated to the Cookbook are represented by:

[!code-csharp[](../../Chefs/Presentation/CreateUpdateCookbookModel.cs#L58-L62)]

When creating or editing a Cookbook we need to have access to the selected recipes that will be linked to the Cookbook. In order to dynamically access it we use the `.Selection()` method to link the selected recipes to the `SelectedRecipes` property:

```csharp
public IState<IImmutableList<Recipe>> SelectedRecipes { get; }

// Code omitted for brevity

public IListFeed<Recipe> Recipes => ListFeed
	.PaginatedAsync(
		async (PageRequest pageRequest, CancellationToken ct) =>
			await _recipeService.GetFavoritedWithPagination(pageRequest.DesiredSize ?? DefaultPageSize, pageRequest.CurrentCount, ct)
	)
	.Selection(SelectedRecipes);
```

Now, when saving the changes we can dynamically access the selected recipes:

[!code-csharp[](../../Chefs/Presentation/CreateUpdateCookbookModel.cs#L65-L87)]

## Source Code

Chefs app

- [CreateUpdateCookbookModel.cs](https://github.com/unoplatform/uno.chefs/blob/139edc9eab65b322e219efb7572583551c40ad32/Chefs/Presentation/CreateUpdateCookbookModel.cs#L63)

## Documentation

- [MVUX Selection](xref:Uno.Extensions.Mvux.Advanced.Selection)
