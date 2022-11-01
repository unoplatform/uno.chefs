using Chefs.Business;
using System.Collections.Immutable;

namespace Chefs.Presentation;

public partial class IngredientsViewModel
{
    private INavigator _navigator;

    public IngredientsViewModel(Recipe recipe, INavigator navigator)
    {
        _navigator = navigator;

        Steps = ListState.Value(this, () => recipe.Ingredients ?? ImmutableList<Ingredient>.Empty);
    }

    public IListState<Ingredient> Steps { get; }

    private async ValueTask GoBack(CancellationToken ct) =>
        await _navigator.GoBack(this);
}
