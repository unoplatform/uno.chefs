using Chefs.Presentation.Extensions;
using Uno.Extensions.Navigation;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Chefs.Presentation;

public partial class SearchModel
{
	private readonly INavigator _navigator;
	private readonly IRecipeService _recipeService;
	private bool hideSearches = false;

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

	public IState<bool> IsSearchesClosed => State<bool>.Value(this, () => hideSearches);

	public IFeed<bool> Searched => Feed.Combine(Filter, Term).Select(GetSearched);

	public IListFeed<Recipe> Recommended => ListFeed.Async(_recipeService.GetRecommended);

	public IListFeed<Recipe> FromChefs => ListFeed.Async(_recipeService.GetFromChefs);

	public IListFeed<string> SearchHistory => ListFeed.Async(async ct => _recipeService.GetSearchHistory());

	public async ValueTask ApplyHistory(string term, CancellationToken ct)
	{
		await Term.Update(s => term, ct);
	}

	private IFeed<IImmutableList<Recipe>> Results => Term
		.SelectAsync(_recipeService.Search);

	private IImmutableList<Recipe> ApplyFilter((IImmutableList<Recipe> recipes, SearchFilter filter) inputs)
		=> inputs.recipes.Where(p => inputs.filter.Match(p)).ToImmutableList();

	private bool GetSearched((SearchFilter filter, string term) inputs) => inputs.filter.HasFilter ? true : !inputs.term.IsNullOrEmpty();

	public async ValueTask CloseSearches(CancellationToken ct)
	{
		await IsSearchesClosed.Update(_ => !hideSearches, ct);
	}

	public async ValueTask SearchPopular(CancellationToken ct) =>
		await _navigator.NavigateViewModelAsync<SearchModel>(this, data: new SearchFilter(OrganizeCategory.Popular, null, null, null, null));

	public async ValueTask ShowCurrentProfile()
	{
		await _navigator.NavigateToProfile(this);
	}

	public async ValueTask ShowNotifications()
	{
		_ = _navigator.NavigateRouteAsync(this, "Notifications", qualifier: Qualifiers.Dialog);
	}
}
