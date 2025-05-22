using Chefs.Business.Services.Recipes;

namespace Chefs.Presentation;

public partial record SearchModel
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

	public IFeed<bool> Searched => Feed.Combine(Filter, Term).Select(GetSearched);

	public IFeed<bool> HasFilter => Filter.Select(f => f.HasFilter);

	public IListFeed<Recipe> Recommended => ListFeed.Async(_recipeService.GetRecommended);

	public IListFeed<Recipe> FromChefs => ListFeed.Async(_recipeService.GetFromChefs);

	public async ValueTask ApplyHistory(string term) => await Term.SetAsync(term);

	private async ValueTask<IImmutableList<Recipe>> Search((string Term, SearchFilter Filter) inputs, CancellationToken ct)
	{
		var searchedRecipes = await _recipeService.Search(inputs.Term, inputs.Filter, ct);
		return searchedRecipes.Where(inputs.Filter.Match).ToImmutableList();
	}

	private bool GetSearched((SearchFilter Filter, string Term) inputs) => inputs.Filter.HasFilter || !inputs.Term.IsNullOrEmpty();

	public async ValueTask SearchPopular() =>
		await _navigator.NavigateViewModelAsync<SearchModel>(this, data: new SearchFilter(FilterGroup: FilterGroup.Popular));

	public async ValueTask ResetFilters() =>
		await Filter.UpdateAsync(current => new SearchFilter());
}
