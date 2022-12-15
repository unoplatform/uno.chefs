using Chefs.Business;
using System.Collections.Immutable;

namespace Chefs.Presentation;

public partial class SearchModel // DR_REV: Use Model suffix instead of ViewModel
{
    private readonly INavigator _navigator;
    private readonly IRecipeService _recipeService;
    private Signal _searchSignal = new();

    public SearchModel(SearchFilter? filter, INavigator navigator, IRecipeService recipeService)
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

    // DR_REV: Duplicate with property above
    public IListFeed<Recipe> FromChefs => ListFeed.Async(_recipeService.GetRecommended);

    //private IFeed<IImmutableList<Recipe>> Results => Term.SelectAsync(_recipeService.Search);

    public IListState<string> SearchHistory => ListState.Value(this, () => _recipeService.GetSearchHistory());

    public async ValueTask ApplySearchHistory(string term, CancellationToken ct)
    {
        await Term.Update(s => term, ct);
        await Search(ct);
    }

    private IFeed<IImmutableList<Recipe>> Results => Feed.Async(async ct => 
    {
        var term = await Term;
        return term.IsNullOrEmpty() ? ImmutableList<Recipe>.Empty : await _recipeService.Search(term ?? string.Empty, ct);
    }, _searchSignal);

    private IImmutableList<Recipe> ApplyFilter((IImmutableList<Recipe> recipes, SearchFilter filter) inputs) 
		=> inputs.recipes.Where(p => inputs.filter.Match(p)).ToImmutableList();

    private bool GetSearched((SearchFilter filter, IImmutableList<Recipe> recipes) inputs) => inputs.filter.HasFilter ? true : inputs.recipes.Count > 0;
     
    public async ValueTask GoBack(CancellationToken ct) =>
        await _navigator.GoBack(this);

    public async ValueTask RecipeDetails(Recipe recipe, CancellationToken ct) =>
        await _navigator.NavigateViewModelAsync<RecipeDetailsModel>(this, data: recipe);

    public async ValueTask GoToFilter(CancellationToken ct) 
    {
        var response = await _navigator.GetDataAsync<FilterModel, SearchFilter>(this, data: await Filter, qualifier: Qualifiers.Dialog, cancellation: ct);

        if (response is not null)
        {
            await Filter.Update(current => response, ct);
        }
    }

    public async ValueTask Search(CancellationToken ct) => _searchSignal.Raise();

}