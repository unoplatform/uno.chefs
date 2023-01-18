using Chefs.Business;

namespace Chefs.Presentation;

public partial class FilterModel
{
	private readonly INavigator _navigator;
    private readonly IRecipeService _recipeService;

    public FilterModel(SearchFilter filters, INavigator navigator, IRecipeService recipeService)
    {
        _navigator = navigator;
        _recipeService = recipeService;

        Filter = State.Value(this, () => filters);
    }

    public IState<SearchFilter> Filter { get; }

    public IListFeed<Category> Categories => ListFeed.Async(_recipeService.GetCategories);

    public async ValueTask SearchFilter(SearchFilter filter, CancellationToken ct) =>
        await _navigator.NavigateBackWithResultAsync(this, data: filter, cancellation: ct);

    public async ValueTask ResetFilter(CancellationToken ct) =>
       await Filter.Update(current => new SearchFilter(null, null, null, null, null), ct);
}
