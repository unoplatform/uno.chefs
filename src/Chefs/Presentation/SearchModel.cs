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

	public IFeed<bool> HasFilter => Filter.Select(f => f.HasFilter);

	public IListFeed<Recipe> Recommended => ListFeed.Async(_recipeService.GetRecommended);

	public IListFeed<Recipe> FromChefs => ListFeed.Async(_recipeService.GetFromChefs);

	public IListFeed<string> SearchHistory => ListFeed.Async(async ct => _recipeService.GetSearchHistory());

	public async ValueTask ApplyHistory(string term) => await Term.SetAsync(term);
	
	private async ValueTask<IImmutableList<Recipe>> Search((string term, SearchFilter filter) inputs, CancellationToken ct)
	{
		var searchedRecipes = await _recipeService.Search(inputs.term, inputs.filter, ct);
		return searchedRecipes.Where(inputs.filter.Match).ToImmutableList();
	}

	private bool GetSearched((SearchFilter filter, string term) inputs) => inputs.filter.HasFilter || !inputs.term.IsNullOrEmpty();

	public async ValueTask SearchPopular() =>
		await _navigator.NavigateViewModelAsync<SearchModel>(this, data: new SearchFilter(FilterGroup: FilterGroup.Popular));

	public async ValueTask ResetFilters() =>
		await Filter.UpdateAsync(current => new SearchFilter());
}
