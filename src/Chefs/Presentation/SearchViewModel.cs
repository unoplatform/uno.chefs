using Chefs.Business;
using System.Collections.Immutable;

namespace Chefs.Presentation;

public partial class SearchViewModel
{
    private INavigator _navigator;
    private IRecipeService _recipeService;
    private Signal _searchSignal = new();

    public SearchViewModel(SearchFilter? filter, INavigator navigator, IRecipeService recipeService)
    {
        _navigator = navigator;
        _recipeService = recipeService;

        Filter = State.Value(this, () => filter ?? new SearchFilter(null, null, null, null, null));
    }

    public IState<string> Term => State<string>.Value(this, () => string.Empty);

    public IState<SearchFilter> Filter { get; }

    public IListFeed<Recipe> Items => Feed
        .Combine(Results, Filter)
        .Select(ApplyFilter)
        .AsListFeed();

    public IFeed<bool> Searched => Feed.Combine(Filter, Results).Select(GetSearched);

    public IListFeed<Recipe> Recommended => ListFeed.Async(_recipeService.GetRecommended);

    public IListFeed<Recipe> FromChefs => ListFeed.Async(_recipeService.GetRecommended);

    private IFeed<IImmutableList<Recipe>> Results => Feed.Async(async ct => 
    {
        var term = await Term;
        return term.IsNullOrEmpty() ? ImmutableList<Recipe>.Empty : await _recipeService.Search(term ?? string.Empty, ct);
    }, _searchSignal);

    private IImmutableList<Recipe> ApplyFilter((IImmutableList<Recipe> recipes, SearchFilter filter) inputs) =>
        inputs.recipes.Where(p => inputs.filter.Match(p)).ToImmutableList();

    private bool GetSearched((SearchFilter filter, IImmutableList<Recipe> recipes) inputs) => inputs.filter.HasFilter ? true : inputs.recipes.Count > 0;
     
    public async ValueTask GoBack(CancellationToken ct) =>
        await _navigator.GoBack(this);

    public async ValueTask RecipeDetails(Recipe recipe, CancellationToken ct) =>
        await _navigator.NavigateViewModelAsync<RecipeDetailsViewModel>(this, data: recipe);

    public async ValueTask GoToFilter(CancellationToken ct) 
    {
        var response = await _navigator.GetDataAsync<FilterViewModel, SearchFilter>(this, data: await Filter, qualifier: Qualifiers.Dialog, cancellation: ct);

        if (response is not null)
        {
            await Filter.Update(current => response, ct);
        }
    }

    public async ValueTask Search(CancellationToken ct) => _searchSignal.Raise();

}