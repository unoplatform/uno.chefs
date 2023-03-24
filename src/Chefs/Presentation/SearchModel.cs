using Chefs.Business;
using System.Collections.Immutable;
using Uno.Extensions;

namespace Chefs.Presentation;

public partial class SearchModel
{
    private readonly INavigator _navigator;
    private readonly IRecipeService _recipeService;
    private Signal _searchSignal = new();
    private bool hideSerches = false;

    public SearchModel(SearchFilter? filter, INavigator navigator, IRecipeService recipeService)
    {
        _navigator = navigator;
        _recipeService = recipeService;

        Filter = State.Value(this, () => filter ?? new SearchFilter(null, null, null, null, null));
    }

    public IState<string> Term => State<string>.Value(this, () => string.Empty);

    public IState<SearchFilter> Filter { get; }

    public IListFeed<Recipe> Items => Feed
        .Combine(Results, Filter.Select(f => f))
        .Select(ApplyFilter)
        .AsListFeed();

    public IState<bool> IsSearchesClosed => State<bool>.Value(this, () => hideSerches);

    public IFeed<bool> Searched => Feed.Combine(Filter, Term).Select(GetSearched);

    public IListFeed<Recipe> Recommended => ListFeed.Async(_recipeService.GetRecommended);

    public IListFeed<Recipe> FromChefs => ListFeed.Async(_recipeService.GetFromChefs);

    public IListFeed<string> SearchHistory => ListFeed.Async(async ct => _recipeService.GetSearchHistory());

    public async ValueTask ApplySearchHistory(string term, CancellationToken ct)
    {
        await Term.Update(s => term, ct);
        await Search(ct);
    }

    private IFeed<IImmutableList<Recipe>> Results => Term
        .SelectAsync(_recipeService.Search);

    private IImmutableList<Recipe> ApplyFilter((IImmutableList<Recipe> recipes, SearchFilter filter) inputs) 
		=> inputs.recipes.Where(p => inputs.filter.Match(p)).ToImmutableList();

    private bool GetSearched((SearchFilter filter, string term) inputs) => inputs.filter.HasFilter ? true : !inputs.term.IsNullOrEmpty();
     
    public async ValueTask CloseRecentSearches(CancellationToken ct)
    {
        await IsSearchesClosed.Update(_ => !hideSerches, ct);
    }    

    public async ValueTask RecipeDetails(Recipe recipe, CancellationToken ct) =>
        await _navigator.NavigateViewModelAsync<RecipeDetailsModel>(this, data: recipe);

    public async ValueTask Search(CancellationToken ct) => _searchSignal.Raise();

}