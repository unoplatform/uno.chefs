using Chefs.Business;

namespace Chefs.Presentation;

public partial class FilterViewModel
{
    private INavigator _navigator;
    private IRecipeService _recipeService;

    public FilterViewModel(SearchFilter filters, INavigator navigator, IRecipeService recipeService)
    {
        _navigator = navigator;
        _recipeService = recipeService;

        Filter = State.Value(this, () => filters);
    }

    public IState<SearchFilter> Filter;

    public IListFeed<Category> Categories => ListFeed.Async(_recipeService.GetCategories);

    public async ValueTask SearchFilter(CancellationToken ct) =>
        await _navigator.NavigateBackWithResultAsync(this, data: Filter, cancellation: ct);
}
