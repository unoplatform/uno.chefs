using Chefs.Business;
using Chefs.Data;

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

    public IEnumerable<OrganizeCategories> OrganizeCategories => Enum.GetValues(typeof(OrganizeCategories)).Cast<OrganizeCategories>();
    public IEnumerable<Times> Times => Enum.GetValues(typeof(Times)).Cast<Times>();
    public IEnumerable<Difficulties> Difficulties => Enum.GetValues(typeof(Difficulties)).Cast<Difficulties>();
    public IEnumerable<int> Serves => new int[] { 1, 2, 3, 4, 5 };

    public IListFeed<Category> Categories => ListFeed.Async(_recipeService.GetCategories);

    public async ValueTask SearchFilter(SearchFilter filter, CancellationToken ct) =>
        await _navigator.NavigateBackWithResultAsync(this, data: filter, cancellation: ct);

    public async ValueTask ResetFilter(CancellationToken ct) =>
       await Filter.Update(current => new SearchFilter(null, null, null, null, null), ct);
}
