using Chefs.Presentation.Extensions;

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

		Filter = State.Value(this, () => filter ?? new SearchFilter());
	}

	public IState<string> Term => State<string>.Value(this, () => string.Empty);

	public IState<SearchFilter> Filter { get; }

	public IListFeed<Recipe> Items => Feed
		.Combine(Results, Filter)
		.Select(ApplyFilter)
		.AsListFeed<Recipe>();

	public IFeed<bool> Searched => Feed.Combine(Filter, Term).Select(GetSearched);

	public IFeed<bool> HasFilter => Filter.Select(f => f.HasFilter);

	public IListFeed<Recipe> Recommended => ListFeed.Async(_recipeService.GetRecommended);

	public IListFeed<Recipe> FromChefs => ListFeed.Async(_recipeService.GetFromChefs);

	public IListFeed<string> SearchHistory => ListFeed.Async(async ct => _recipeService.GetSearchHistory());

	public async ValueTask ApplyHistory(string term)
	{
		await Term.SetAsync(term);
	}

	private IFeed<IImmutableList<Recipe>> Results => Term
		.SelectAsync(_recipeService.Search);

	private IImmutableList<Recipe> ApplyFilter((IImmutableList<Recipe> recipes, SearchFilter filter) inputs)
	{
		IImmutableList<Recipe> recipesByCategory = inputs.recipes;

		if (inputs.filter.OrganizeCategory is OrganizeCategory category)
		{
			recipesByCategory = FilterByCategory(category);
		}

		return recipesByCategory.Intersect(inputs.recipes).Where(p => inputs.filter.Match(p)).ToImmutableList();
	}

	private IImmutableList<Recipe> FilterByCategory(OrganizeCategory category)
	{
		return category switch
		{
			OrganizeCategory.Popular => _recipeService.GetPopular(new CancellationToken()).Result,
			OrganizeCategory.Trending => _recipeService.GetTrending(new CancellationToken()).Result,
			OrganizeCategory.Recent => _recipeService.GetRecent(new CancellationToken()).Result,
			_ => ImmutableList<Recipe>.Empty
		};
	}

	private bool GetSearched((SearchFilter filter, string term) inputs) => inputs.filter.HasFilter || !inputs.term.IsNullOrEmpty();

	public async ValueTask SearchPopular() =>
		await _navigator.NavigateViewModelAsync<SearchModel>(this, data: new SearchFilter(OrganizeCategory: OrganizeCategory.Popular));

	public async ValueTask ShowCurrentProfile() => 
		await _navigator.NavigateToProfile(this);

	public async ValueTask ShowNotifications() => 
		await _navigator.NavigateToNotifications(this);

	public async ValueTask ResetFilters() =>
		await Filter.UpdateAsync(current => new SearchFilter());
}
