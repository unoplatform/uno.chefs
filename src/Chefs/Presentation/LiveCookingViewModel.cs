using Chefs.Business;
using Chefs.Data;
using System.Collections.Immutable;

namespace Chefs.Presentation;

public record LiveCookingParameter(Recipe Recipe, IImmutableList<Step> Steps);

public partial class LiveCookingViewModel
{
    private INavigator _navigator;

    public LiveCookingViewModel(LiveCookingParameter parameter, INavigator navigator)
    {
        _navigator = navigator;
        Steps = ListState.Value(this, () => parameter.Steps);
        Recipe = State.Value(this, () => parameter.Recipe);
    }

    public IListFeed<Step> Steps { get; }

    public IState<Recipe> Recipe { get; }

    public IState<int> SelectedIndex => State.Value(this, () => 0);

    public async ValueTask GoBack(CancellationToken ct) =>
        await _navigator.GoBack(this);
}
