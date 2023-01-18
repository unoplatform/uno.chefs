using Chefs.Business;
using System.Collections.Immutable;

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

    public IFeed<Step?> SelectedStep => Feed.Combine(Steps, SelectedIndex).Select(param => param.Item2 < 0 ? param.Item1[param.Item2] : param.Item1.FirstOrDefault());

    public IState<int> SelectedIndex => State.Value(this, () => 0);

    public async ValueTask GoBack(CancellationToken ct) =>
        await _navigator.GoBack(this);
}
