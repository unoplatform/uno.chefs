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

	public IListFeed<Recipe> Items => Feed
		.Combine(Results, Filter)
		.Select(ApplyFilter)
		.AsListFeed();

	public IFeed<bool> Searched => Feed.Combine(Filter, Term).Select(GetSearched);

	public IFeed<bool> HasFilter => Filter.Select(f => f.HasFilter);

	public IListFeed<Recipe> Recommended => ListFeed.Async(_recipeService.GetRecommended);

	public IListFeed<Recipe> FromChefs => ListFeed.Async(_recipeService.GetFromChefs);

	public IListFeed<string> SearchHistory => ListFeed.Async(async ct => _recipeService.GetSearchHistory());

	public async ValueTask ApplyHistory(string term) => await Term.SetAsync(term);

	private IFeed<IImmutableList<Recipe>> Results => Feed
		.Combine(Term, Filter.Select(f => f.FilterGroup))
		.SelectAsync(_recipeService.Search);
	
	private IImmutableList<Recipe> ApplyFilter((IImmutableList<Recipe> recipesToFilter, SearchFilter filter) inputs)
	{
		return inputs.recipesToFilter.Where(inputs.filter.Match).ToImmutableList();
	}

	private bool GetSearched((SearchFilter filter, string term) inputs) => inputs.filter.HasFilter || !inputs.term.IsNullOrEmpty();

	public async ValueTask SearchPopular() =>
		await _navigator.NavigateViewModelAsync<SearchModel>(this, data: new SearchFilter(FilterGroup: FilterGroup.Popular));

	public async ValueTask ResetFilters() =>
		await Filter.UpdateAsync(current => new SearchFilter());
}
