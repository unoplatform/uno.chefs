using Chefs.Business;
using Chefs.Data;
using System.Collections.Immutable;

namespace Chefs.Presentation;

public partial class LiveCookingViewModel
{
    private INavigator _navigator;
    private int _index = 0;

    public LiveCookingViewModel(IImmutableList<Step> steps, INavigator navigator)
    {
        _navigator = navigator;
        Steps = ListState.Value(this, () => steps);
    }

    public IListState<Step> Steps { get; }

    public IState<Step> CurrentStep => State.Async(this, async ct => (await Steps)[_index]);

    public async ValueTask GoBack(CancellationToken ct) =>
        await _navigator.GoBack(this);
}
