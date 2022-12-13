using Chefs.Business;

namespace Chefs.Presentation;

public partial class FilterViewModel // DR_REV: Use Model suffix instead of ViewModel
{
	private readonly INavigator _navigator;
    private readonly IRecipeService _recipeService;
    private readonly SearchFilter _filter;

    public FilterViewModel(SearchFilter filters, INavigator navigator, IRecipeService recipeService)
    {
        _navigator = navigator;
        _recipeService = recipeService;

        _filter = filters;
    }

    public IState<SearchFilter> Filter => State.Value(this, () => _filter);

    public IListFeed<Category> Categories => ListFeed.Async(_recipeService.GetCategories);

    // DR_REV: Use implicit parameters
    public async ValueTask SearchFilter(SearchFilter filter, CancellationToken ct) =>
        await _navigator.NavigateBackWithResultAsync(this, data: filter, cancellation: ct);

    public async ValueTask ResetFilter(CancellationToken ct) =>
       await Filter.Update(current => new SearchFilter(null, null, null, null, null), ct);
}
