using Chefs.Business;

namespace Chefs.Presentation;

public partial class FilterViewModel
{
    private INavigator _navigator;
    private IRecipeService _recipeService;
    private SearchFilter _filter;

    public FilterViewModel(SearchFilter filters, INavigator navigator, IRecipeService recipeService)
    {
        _navigator = navigator;
        _recipeService = recipeService;

        _filter = filters;
    }

    public IState<SearchFilter> Filter => State.Value(this, () => _filter);

    public IListFeed<Category> Categories => ListFeed.Async(_recipeService.GetCategories);

    public async ValueTask SearchFilter(CancellationToken ct) =>
        await _navigator.NavigateBackWithResultAsync(this, data: await Filter, cancellation: ct);

    public async ValueTask ResetFilter(CancellationToken ct) =>
       await Filter.Update(current => new SearchFilter(null, null, null, null), ct);
}
