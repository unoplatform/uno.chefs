---
uid: Uno.Recipes.ReactiveSearch
---

# How to Create a Reactive Search Experience with MVUX

## Problem

Filtering search results live as the user types is a common feature in modern applications. This requires updating the search results every time the search term changes, either per key-press or on submission. Without a proper mechanism, managing this can be complex and inefficient.

## Solution

The **Uno.Extensions** library, specifically MVUX, provides a seamless way to live filter search results using `IState<string>` and `SelectAsync` on the IState to react to state changes. This allows for a dynamic and responsive search experience.

### Using MVUX to Create a Reactive Search Experience

In the Chefs app, for the Search page, we use `IState<string>` to hold the search term and `IListFeed<Recipe>` for the search results. The search results are updated every time the search term changes.

Below is a simplified version of the way search is done in the Chefs app. See the Source Code section below for references to the full implementation.

1. `SearchModel.cs`

    ```csharp
    namespace Chefs.Presentation;

    public partial class SearchModel
    {
        private readonly INavigator _navigator;
        private readonly IRecipeService _recipeService;

        public SearchModel(SearchFilter? filter, INavigator navigator, IRecipeService recipeService)
        {
            _navigator = navigator;
            _recipeService = recipeService;

            Filter = State.Value(this, () => filter ?? new SearchFilter());
        }

        public IState<string> Term => State<string>.Value(this, () => string.Empty);

        public IState<SearchFilter> Filter { get; }

        public IListFeed<Recipe> Results => Feed
            .Combine(Term, Filter)
            .SelectAsync(Search)
            .AsListFeed();

        public IFeed<bool> Searched => Feed.Combine(Filter, Term).Select(GetSearched);

        private async ValueTask<IImmutableList<Recipe>> Search((string term, SearchFilter filter) inputs, CancellationToken ct)
        {
            var searchedRecipes = await _recipeService.Search(inputs.term, inputs.filter, ct);
            return searchedRecipes.Where(inputs.filter.Match).ToImmutableList();
        }
    }
    ```

    The `Search` method in the `IRecipeService` is called whenever the Term state changes, and it returns the filtered list of recipes.

2. `SearchPage.xaml`

    ```xml
    <TextBox
        PlaceholderText="Search"
        Text="{Binding Term, Mode=TwoWay, UpdateSourceTrigger=PropertyChanged}" />

    <ScrollViewer utu:AutoLayout.PrimaryAlignment="Stretch"
                VerticalScrollBarVisibility="Hidden">
        <uer:FeedView x:Name="SearchFeed"
                    NoneTemplate="{StaticResource EmptyTemplate}"
                    Source="{Binding Results}">
            <DataTemplate>
                <!-- Code omitted for brevity -->
            </DataTemplate>
        </uer:FeedView>
    </ScrollViewer>
    ```
    The `FeedView` control automatically updates the displayed list of recipes whenever the `Results` feed is updated, providing a dynamic and responsive search experience.

### Using Custom Filter Logic

In addition to the search term, you can also maintain a `filter` state to refine search results further. The `Filter` property in the `SearchModel` allows you to define custom filter logic.

The `Filter` property is explained in a separate Recipe Book entry on [how to use custom filter logic as part of a reactive search experience](xref:).

## Source Code

Chefs app

- [Search Model](https://github.com/unoplatform/uno.chefs/blob/92105f64923058b9ace3897bbea17cdb3b354fe9/src/Chefs/Presentation/SearchModel.cs#L22)

- [Search Page](https://github.com/unoplatform/uno.chefs/blob/d226a4baf0e04641b23e9f8324112b05c47abfde/src/Chefs/Views/SearchPage.xaml)

## Documentation

- [MVUX FeedView](xref:Uno.Extensions.Mvux.FeedView)
