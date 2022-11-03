using Chefs.Business;
using System.Collections.Immutable;

namespace Chefs.Presentation;

public partial class SearchViewModel
{
    private INavigator _navigator;
    private IRecipeService _recipeService;

    public SearchViewModel(SearchFilter? filters, INavigator navigator, IRecipeService recipeService)
    {
        _navigator = navigator;
        _recipeService = recipeService;

        Filter = State.Value(this, () => filters ?? new SearchFilter(null, null, null, null));
    }

    public IState<string> Term => State<string>.Value(this, () => string.Empty);

    public IState<SearchFilter> Filter { get; }

    public IListFeed<Recipe> Items => Feed
        .Combine(Results, Filter)
        .Select(ApplyFilter)
        .AsListFeed();

    public async ValueTask SearchFilter(CancellationToken ct) =>
        await _navigator.GetDataAsync<FilterViewModel>(this, data: Filter, cancellation: ct);

    private IFeed<IImmutableList<Recipe>> Results => Term
        .SelectAsync(_recipeService.Search);

    private IImmutableList<Recipe> ApplyFilter((IImmutableList<Recipe> recipes, SearchFilter filter) inputs) =>
        inputs.recipes.Where(p => inputs.filter.Match(p)).ToImmutableList();

    public async ValueTask GoBack(CancellationToken ct) =>
        await _navigator.GoBack(this);

    public async ValueTask RecipeDetails(Recipe recipe, CancellationToken ct) =>
        await _navigator.NavigateViewModelAsync<RecipeDetailsViewModel>(this, data: recipe);

    public async ValueTask GoToFilter(CancellationToken ct) =>
        await _navigator.NavigateViewModelAsync<FilterViewModel>(this, data: Filter);
}