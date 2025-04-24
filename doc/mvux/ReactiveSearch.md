---
uid: Uno.Recipes.ReactiveSearch
---

# How to Build a Real-Time Search with MVUX

## Problem

Displaying live search results as the user types is a common feature in modern applications. This requires updating the search results every time the search term changes, either per key-press or on submission. Without a proper mechanism, managing this can be complex and inefficient.

## Solution

**Uno.Extensions.MVUX** provides a seamless way to live update search results using `IState<string>` and `SelectAsync` on the IState to react to state changes. This allows for a dynamic and responsive search experience.

### Using MVUX to Create a Reactive Search Experience

#### 1. `SearchModel.cs`

```csharp
public partial class SearchModel
{
    private readonly INavigator _navigator;
    private readonly IRecipeService _recipeService;
    private readonly IMessenger _messenger;

    public SearchModel(SearchFilter? filter, INavigator navigator, IRecipeService recipeService, IMessenger messenger)
    {
        _navigator = navigator;
        _recipeService = recipeService;
        _messenger = messenger;

        Filter = State.Value(this, () => filter ?? new SearchFilter())
            .Observe(_messenger, f => f);
    }

    public IState<string> Term => State<string>.Value(this, () => string.Empty)
        .Observe(_messenger, t => t);

    public IState<SearchFilter> Filter { get; }

    public IListState<Recipe> Results => ListState.FromFeed(this, Feed
        .Combine(Term, Filter)
        .SelectAsync(Search)
        .AsListFeed())
        .Observe(_messenger, r => r.Id);

    ...

    private async ValueTask<IImmutableList<Recipe>> Search((string term, SearchFilter filter) inputs, CancellationToken ct)
    {
        var searchedRecipes = await _recipeService.Search(inputs.term, inputs.filter, ct);
        return searchedRecipes.Where(inputs.filter.Match).ToImmutableList();
    }
}
```

The `Search` method in the `IRecipeService` is called whenever the **Term** state changes, and it returns the filtered list of recipes.

#### 2. `SearchPage.xaml`

Search Term

[!code-xml[](../../Chefs/Views/SearchPage.xaml#L114-L118)]

FeedView

[!code-xml[](../../Chefs/Views/SearchPage.xaml#L161-L177)]

The `FeedView` control automatically updates the displayed list of recipes whenever the `Results` feed is updated, providing a dynamic and responsive search experience.

### Using Custom Filter Logic

In addition to the search term, you can also maintain a **filter state** to refine search results further. In Chefs, the `Filter` property in the `SearchModel` defines custom filtering logic. See [How to Filter Search Results Dynamically with MVUX](xref:Uno.Recipes.SearchFilters) for a closer look.

## Source Code

Chefs app

- [SearchModel.cs](https://github.com/unoplatform/uno.chefs/blob/139edc9eab65b322e219efb7572583551c40ad32/Chefs/Presentation/SearchModel.cs)
- [SearchPage.xaml (Search Term)](https://github.com/unoplatform/uno.chefs/blob/139edc9eab65b322e219efb7572583551c40ad32/Chefs/Views/SearchPage.xaml#L114-L118)
- [SearchPage.xaml (FeedView)](https://github.com/unoplatform/uno.chefs/blob/139edc9eab65b322e219efb7572583551c40ad32/Chefs/Views/SearchPage.xaml#L161-L177)

## Documentation

- [MVUX FeedView](xref:Uno.Extensions.Mvux.FeedView)
