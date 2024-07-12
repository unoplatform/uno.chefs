---
uid: Uno.Recipes.CustomFilterLogic
---

# How to Use Custom Filter Logic as Part of a Reactive Search Experience

## Problem

Filtering search results live as the user types is a common feature in modern applications. To refine search results further, custom filter logic can be implemented. Managing this efficiently across different platforms can be complex.

## Solution

The **Uno.Extensions** library, specifically MVUX, provides a way to create a reactive search experience with custom filter logic using `IState<SearchFilter>` react to state changes. This allows for a dynamic and responsive search experience with advanced filtering options.

### Using Custom Filter Logic in a Reactive Search Experience

1. In the Chefs app, we use `IState<string>` to hold the search term, `IState<SearchFilter>` to hold the filter criteria, and `IListFeed<Recipe>` for the search results. The search results are updated every time the search term or filter criteria changes.

    ``` csharp
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

        private bool GetSearched((SearchFilter filter, string term) inputs) => inputs.filter.HasFilter || !inputs.term.IsNullOrEmpty();
    }
    ```
    The Search method in the IRecipeService is called whenever the `Term` or `Filter` state changes, and it returns the filtered list of recipes.

2. In the `SearchPage.xaml` the `FeedView` control automatically updates the displayed list of recipes whenever the Results feed is updated, providing a dynamic and responsive search experience.

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

## Source Code

Chefs app

- [Search Model](https://github.com/unoplatform/uno.chefs/blob/92105f64923058b9ace3897bbea17cdb3b354fe9/src/Chefs/Presentation/SearchModel.cs#L22)

- [Search Page](https://github.com/unoplatform/uno.chefs/blob/d226a4baf0e04641b23e9f8324112b05c47abfde/src/Chefs/Views/SearchPage.xaml)

## Documentation

- [MVUX FeedView](xref:Uno.Extensions.Mvux.FeedView)
