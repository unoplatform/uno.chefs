using Chefs.Business;
using Chefs.Data;
using System.Collections.Immutable;
using Uno.Extensions.Reactive;

namespace Chefs.Presentation;

public record LiveCookingParameter(Recipe Recipe, IImmutableList<Step> Steps);

public partial class LiveCookingViewModel
{
    private INavigator _navigator;

    public LiveCookingViewModel(LiveCookingParameter parameter, INavigator navigator)
    {
        _navigator = navigator;
        Steps = State.Value(this, () => parameter.Steps);
        Recipe = State.Value(this, () => parameter.Recipe);
    }

    public IState<IImmutableList<Step>> Steps { get; }

    public IState<Recipe> Recipe { get; }

    public IFeed<Step> SelectedStep => Feed.Combine(Steps, SelectedIndex).Select(param => param.Item1[param.Item2]);

    public IState<int> SelectedIndex => State.Value(this, () => 0);

    public async ValueTask GoBack(CancellationToken ct) =>
        await _navigator.GoBack(this);
}
