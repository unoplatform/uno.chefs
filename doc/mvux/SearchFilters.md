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

[!code-csharp[](../../Chefs/Presentation/FilterModel.cs#L3-L19)]

#### 2. `FiltersPage.xaml`

For each recipe filter, we define an `ItemsRepeater` that displays the possible values that filter can take. Each filter will display its possible values following the `FilterChipTemplate` resource defined at the page level.

[!code-xml[](../../Chefs/Views/FiltersPage.xaml#L17-L26)]

[!code-xml[](../../Chefs/Views/FiltersPage.xaml#L51-L64)]

When the user is done selecting filters for their search, they invoke the `ApplySearchFilter()` method. This uses `NavigateBackWithResultAsync()` from `Uno.Extensions.Navigation`. This will redirect the user to the previous page (the search page) while injecting the chosen filters into the search model. See [How to Navigate with Code Behind](Uno.Recipes.NavigationCodeBehind) for more information.

[!code-csharp[](../../Chefs/Presentation/FilterModel.cs#L21-L22)]

## Souce Code

Chefs app

- [FilterModel.cs](https://github.com/unoplatform/uno.chefs/blob/139edc9eab65b322e219efb7572583551c40ad32/Chefs/Presentation/FilterModel.cs)
- [FiltersPage.xaml](https://github.com/unoplatform/uno.chefs/blob/139edc9eab65b322e219efb7572583551c40ad32/Chefs/Views/FiltersPage.xaml#L17-L154)

## Documentation

- [MVUX FeedView](xref:Uno.Extensions.Mvux.FeedView)
