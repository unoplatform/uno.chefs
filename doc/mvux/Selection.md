---
uid: Uno.Recipes.Selection
---

# Selecting items from a list with MVUX

## Problem

Selecting items from dynamic lists can be tricky, especially when using paginated data. Doing it manually often adds extra code and makes things more complicated.

## Solution

`Uno.Extensions.MVUX` makes item selection easier by offering a built-in `Selection` method. This lets you manage the selected items with less code and better structure.

### Using Selection

As an example, in Chefs it is possible to create and edit Cookbooks that are a list of recipes. On the **Favorites** page, under the **My Cookbooks** tab, there is a list of existing Cookbooks and the recipes associated to the Cookbook are represented by the Recipes `ListFeed`:

```csharp
public IListFeed<Recipe> Recipes => ListFeed
	.PaginatedAsync(
		async (PageRequest pageRequest, CancellationToken ct) =>
			await _recipeService.GetFavoritedWithPagination(pageRequest.DesiredSize ?? DefaultPageSize, pageRequest.CurrentCount, ct)
	)
	.Selection(SelectedRecipes);
```

When creating or editing a Cookbook we need to have access to the selected recipes that will be linked to the Cookbook. In order to dynamically access it we use the `.Selection()` method to link the selected recipes to the `SelectedRecipes` property. Now, when saving the changes we can dynamically access the selected recipes:

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

		await _navigator.NavigateBackAsync(this);
	}
	else
	{
		await _navigator.ShowDialog(this, new DialogInfo("Error", "Please write a cookbook name and select one recipe."), ct);
	}
}
```

## Source Code

Chefs app

- [CreateUpdateCookbookModel.cs](https://github.com/unoplatform/uno.chefs/blob/139edc9eab65b322e219efb7572583551c40ad32/Chefs/Presentation/CreateUpdateCookbookModel.cs#L63)

## Documentation

- [MVUX Selection](xref:Uno.Extensions.Mvux.Advanced.Selection)
