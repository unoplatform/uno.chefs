---
uid: Uno.Recipes.MVUX.ReactiveSearch
---

# How to Create a Reactive Search Experience with MVUX

## Problem

Filtering search results live as the user types is a common feature in modern applications. This requires updating the search results every time the search term changes, either per key-press or on submission. Without a proper mechanism, managing this can be complex and inefficient.

## Solution

**Uno.Extensions.MVUX** provides a seamless way to live filter search results using `IState<string>` and `SelectAsync` on the IState to react to state changes. This allows for a dynamic and responsive search experience.

### Using MVUX to Create a Reactive Search Experience

In the Chefs search page, we use `IState<string>` to hold the search term and `IListFeed<Recipe>` for the search results. The search results are updated every time the search term changes.

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

    private async ValueTask<IImmutableList<Recipe>> Search((string term, SearchFilter filter) inputs, CancellationToken ct)
    {
        var searchedRecipes = await _recipeService.Search(inputs.term, inputs.filter, ct);
        return searchedRecipes.Where(inputs.filter.Match).ToImmutableList();
    }
}
```

The `Search` method in the `IRecipeService` is called whenever the **Term** state changes, and it returns the filtered list of recipes.

#### 2. `SearchPage.xaml`

```xml
<utu:AutoLayout utu:AutoLayout.PrimaryAlignment="Stretch">
    <utu:AutoLayout>
        <TextBox utu:CommandExtensions.Command="{Binding Search}"
                Style="{StaticResource ChefsPrimaryTextBoxStyle}"
                CornerRadius="28"
                PlaceholderText="Search"
                Text="{Binding Term, Mode=TwoWay, UpdateSourceTrigger=PropertyChanged}">
    </utu:AutoLayout>

    <uer:FeedView x:Name="SearchFeed"
                NoneTemplate="{StaticResource EmptyTemplate}"
                utu:AutoLayout.PrimaryAlignment="Stretch"
                Source="{Binding Results}">
        <DataTemplate>
            <ScrollViewer VerticalScrollBarVisibility="Hidden">
                <muxc:ItemsRepeater x:Name="SearchRepeater"
                                    Margin="{utu:Responsive Narrow='16,0,16,16', Wide='40,0,40,40'}"
                                    uen:Navigation.Request="SearchRecipeDetails"
                                    ItemTemplate="{StaticResource RecipeTemplate}"
                                    ItemsSource="{Binding Data}"
                                    Layout="{StaticResource ResponsiveGridLayout}" />
            </ScrollViewer>
        </DataTemplate>
    </uer:FeedView>
</utu:AutoLayout>
```

The `FeedView` control automatically updates the displayed list of recipes whenever the `Results` feed is updated, providing a dynamic and responsive search experience.

### Using Custom Filter Logic

In addition to the search term, you can also maintain a **filter state** to refine search results further. In Chefs, the `Filter` property in the `SearchModel` defines custom filtering logic.

#### 1. `FilterModel.cs`

```csharp
public partial record FilterModel
{
    private readonly INavigator _navigator;
    private readonly IRecipeService _recipeService;

    public FilterModel(SearchFilter filters, INavigator navigator, IRecipeService recipeService)
    {
        _navigator = navigator;
        _recipeService = recipeService;

        Filter = State.Value(this, () => filters);
    }

    public IState<SearchFilter> Filter { get; }

    // The different possible filters
    public IEnumerable<FilterGroup> FilterGroups => Enum.GetValues(typeof(FilterGroup)).Cast<FilterGroup>();
    public IEnumerable<Time> Times => Enum.GetValues(typeof(Time)).Cast<Time>();
    public IEnumerable<Difficulty> Difficulties => Enum.GetValues(typeof(Difficulty)).Cast<Difficulty>();
    public IEnumerable<int> Serves => new int[] { 1, 2, 3, 4, 5 };
    public IListFeed<Category> Categories => ListFeed.Async(_recipeService.GetCategories);
}
```

#### 2. `FiltersPage.cs`

For each recipe filter, we define an `ItemsRepeater` that displays the possible values that filter can take. Each filter will display its possible values following the `FilterChipTemplate` resource defined at the page level.

```xml
<Page.Resources>
    <DataTemplate x:Key="FilterChipTemplate">
        <utu:Chip Background="{ThemeResource SurfaceBrush}"
                    Content="{Binding}"
                    HorizontalAlignment="Stretch"
                    Foreground="{ThemeResource OnSurfaceVariantBrush}"
                    BorderThickness="1"
                    Style="{StaticResource MaterialChipStyle}" />
    </DataTemplate>
</Page.Resources>

<muxc:ItemsRepeater ItemsSource="{Binding FilterGroups}"
                    utu:ItemsRepeaterExtensions.SelectedItem="{Binding Filter.FilterGroup, Mode=TwoWay}"
                    utu:ItemsRepeaterExtensions.SelectionMode="SingleOrNone"
                    ItemTemplate="{StaticResource FilterChipTemplate}">
    <muxc:ItemsRepeater.Layout>
        <muxc:UniformGridLayout ItemsJustification="Start"
                                MinColumnSpacing="8"
                                MinRowSpacing="8"
                                MinItemWidth="120"
                                ItemsStretch="Fill"
                                MaximumRowsOrColumns="4"
                                Orientation="Horizontal" />
    </muxc:ItemsRepeater.Layout>
</muxc:ItemsRepeater>
```

When the user is done selecting filters for their search, they click on the "Apply filters" button which fires the `ApplySearchFilter()` method. This uses `NavigateBackWithResultAsync()` from `Uno.Extensions.Navigation`. This will redirect the user to the previous page (the search page) while injecting the chosen filters into the search model. See [How to Navigate with Code Behind](Uno.Recipes.NavigationCodeBehind) for more information.

```csharp
public partial record FilterModel
{
    ...

    public async ValueTask ApplySearchFilter(SearchFilter filter) =>
        await _navigator.NavigateBackWithResultAsync(this, data: filter);
}
```

## Source Code

Chefs app

- [SearchModel.cs](https://github.com/unoplatform/uno.chefs/blob/19ace5c583ef4ef55f019589dd1eb07e43000de9/src/Chefs/Presentation/SearchModel.cs#L5-L45)
- [SearchPage.xaml (Search Term)](https://github.com/unoplatform/uno.chefs/blob/19ace5c583ef4ef55f019589dd1eb07e43000de9/src/Chefs/Views/SearchPage.xaml#L114-L118)
- [SearchPage.xaml (FeedView)](https://github.com/unoplatform/uno.chefs/blob/19ace5c583ef4ef55f019589dd1eb07e43000de9/src/Chefs/Views/SearchPage.xaml#L161-L177)
- [FilterModel.cs](https://github.com/unoplatform/uno.chefs/blob/19ace5c583ef4ef55f019589dd1eb07e43000de9/src/Chefs/Presentation/FilterModel.cs#L3C1-L26C2)
- [FiltersPage.xaml](https://github.com/unoplatform/uno.chefs/blob/19ace5c583ef4ef55f019589dd1eb07e43000de9/src/Chefs/Views/FiltersPage.xaml#L17-L154)

## Documentation

- [MVUX FeedView](xref:Uno.Extensions.Mvux.FeedView)
