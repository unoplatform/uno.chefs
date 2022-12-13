using Chefs.Business;
using Chefs.Data;
using System.Collections.Immutable;

namespace Chefs.Presentation;

public record LiveCookingParameter(Recipe Recipe, IImmutableList<Step> Steps);

public partial class LiveCookingViewModel // DR_REV: Use Model suffix instead of ViewModel
{
    private readonly INavigator _navigator;

    public LiveCookingViewModel(LiveCookingParameter parameter, INavigator navigator)
    {
        _navigator = navigator;
        Steps = ListState.Value(this, () => parameter.Steps);
        Recipe = State.Value(this, () => parameter.Recipe);
    }

	// DR_REV: This seems to be used only to display the snapshot received in ctor. There is no need to wrap it into a ListFeed, use a simple property instead:
	//          public IEnumerable<Step> Steps { get; }
	public IListFeed<Step> Steps { get; }

	// DR_REV: This seems to be used only to display the snapshot received in ctor. There is no need to wrap it into a State, use a simple property instead:
	//          public Recipe Recipe { get; }
	public IState<Recipe> Recipe { get; }

	// DR_REV: XAML only nav
	public async ValueTask GoBack(CancellationToken ct) =>
        await _navigator.GoBack(this);
}
