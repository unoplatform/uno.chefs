using Chefs.Business;
using System.Collections.Immutable;

namespace Chefs.Presentation;

public partial class LiveCookingViewModel
{
    private INavigator _navigator;

    public LiveCookingViewModel(IImmutableList<Step> steps, INavigator navigator)
    {
        _navigator = navigator;
        Steps = ListState.Value(this, () => steps);
    }

    public IListState<Step> Steps { get; }

    public IState<int> StepIndex => State.Value(this, () => 0);

    private async ValueTask NextStep(CancellationToken ct) =>
       await StepIndex.Update(x => x++, ct);

    private async ValueTask GoBack(CancellationToken ct) =>
        await _navigator.GoBack(this);
}
