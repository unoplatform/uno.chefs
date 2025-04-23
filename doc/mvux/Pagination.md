---
uid: Uno.Recipes.Pagination
---

# Implementing pagination for lists

## Problem

Loading large lists all at once can hurt performance and user experience. Manually implementing pagination logic adds extra complexity and code that can be hard to maintain.

## Solution

`Uno.Extensions.MVUX` makes pagination easier through the `.PaginatedAsync()` method. This method helps load data in pages and works out-of-the-box with controls that support incremental loading like `ItemsRepeater` or `ListView`.

## Using Pagination

As an example, in Chefs we show a list of favorite recipes when creating or editing a Cookbook. Since the list can be long, we use pagination to load recipes as the user scrolls.

[!code-csharp[](../../Chefs/Presentation/CreateUpdateCookbookModel.cs#L58-L62)]

The `PaginatedAsync` method takes a function that returns the next page of data. It provides the page size and current item count, so we can ask the service only for the new items we need.

In the UI, the pagination works automatically with ItemsRepeater using `SupportsIncrementalLoading="True"`:

```xml
xmlns:uer="using:Uno.Extensions.Reactive.UI"
xmlns:muxc="using:Microsoft.UI.Xaml.Controls"
xmlns:utu="using:Uno.Toolkit.UI"

...

<uer:FeedView Source="{Binding Recipes}">
    <DataTemplate>
        <ScrollViewer>
            <muxc:ItemsRepeater ItemTemplate="{StaticResource CookbookRecipeTemplate}"
                                ItemsSource="{Binding Data}"
                                utu:ItemsRepeaterExtensions.SelectionMode="Multiple"
                                utu:ItemsRepeaterExtensions.SupportsIncrementalLoading="True"
                                ... />
        </ScrollViewer>
    </DataTemplate>
</uer:FeedView>
```

Now, as the user scrolls, more recipes are automatically loaded and displayed — no extra logic required.

## Source Code

Chefs app

- [CreateUpdateCookbookModel.cs](https://github.com/unoplatform/uno.chefs/blob/139edc9eab65b322e219efb7572583551c40ad32/Chefs/Presentation/CreateUpdateCookbookModel.cs#L59-L62)
- [CreateUpdateCookbookPage.xaml](https://github.com/unoplatform/uno.chefs/blob/139edc9eab65b322e219efb7572583551c40ad32/Chefs/Views/CreateUpdateCookbookPage.xaml#L115-L128)

## Documentation

- [MVUX Pagination](xref:Uno.Extensions.Mvux.Advanced.Pagination)
