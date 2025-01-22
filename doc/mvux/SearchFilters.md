---
uid: Uno.Recipes.SearchFilters
---

# How to Create Custom MVUX Search Filters

## Problem

Filtering search results requires updating the displayed results every time the filters are changed. Managing each filter's state and updating the results accordingly can prove to be quite difficult.

## Solution

**Uno.Extensions.MVUX** provides a seamless way to filter live search results using `IState<string>` and `SelectAsync` on the IState to react to state changes. This allows for a dynamic and responsive search experience.

For more details on implementing the search itself, see [How to Create a Reactive Search Experience with MVUX](xref:Uno.Recipes.ReactiveSearch).

### Using Custom Filter Logic

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

When the user is done selecting filters for their search, they invoke the `ApplySearchFilter()` method. This uses `NavigateBackWithResultAsync()` from `Uno.Extensions.Navigation`. This will redirect the user to the previous page (the search page) while injecting the chosen filters into the search model. See [How to Navigate with Code Behind](Uno.Recipes.NavigationCodeBehind) for more information.

```csharp
public partial record FilterModel
{
    ...

    public async ValueTask ApplySearchFilter(SearchFilter filter) =>
        await _navigator.NavigateBackWithResultAsync(this, data: filter);
}
```
