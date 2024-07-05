---
uid: Uno.Recipes.FilteringSearch
---

# How to Filter Search Results with MVUX

## Problem

Filtering search results live as the user types is a common feature in modern applications. This requires updating the search results every time the search term changes, either per key-press or on submission. Without a proper mechanism, managing this can be complex and inefficient.

## Solution

The **Uno.Extensions** library specifically MVUX provides a seamless way to live filter search results using `IState<string>` and `SelectAsync` on the IState to react to term changes. This allows for a dynamic and responsive search experience.

### Using MVUX to Filter Search Results

In the Chefs app, for the Search page, we use `IState<string>` to hold the search term and `IFeed<IImmutableList<Recipe>>` for the search results. The search results are updated every time the search term changes.

1. Define the Term state to hold the search term and the Results feed to perform the search whenever the term changes:

    ``` csharp
    namespace Chefs.Presentation
    {
        public partial class SearchModel
        {
            private readonly IRecipeService _recipeService;

            public SearchModel(IRecipeService recipeService)
            {
                _recipeService = recipeService;
            }

            public IState<string> Term => State<string>.Value(this, () => string.Empty);

            private IFeed<IImmutableList<Recipe>> Results => Term
            .SelectAsync(_recipeService.Search);

            public IListFeed<Recipe> Items => Results.AsListFeed();
        }
    }
    ```
    The `Search` method in the `IRecipeService` is called whenever the Term state changes, and it returns the filtered list of recipes.

2. Bind the Term state to a TextBox and display the filtered results in a FeedView:

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

## Source Code

Chefs app

- [Search Model](https://github.com/unoplatform/uno.chefs/blob/92105f64923058b9ace3897bbea17cdb3b354fe9/src/Chefs/Presentation/SearchModel.cs#L22)

- [Search Page](https://github.com/unoplatform/uno.chefs/blob/d226a4baf0e04641b23e9f8324112b05c47abfde/src/Chefs/Views/SearchPage.xaml)

## Documentation

- [MVUX FeedView](xref:Uno.Extensions.Mvux.FeedView)
