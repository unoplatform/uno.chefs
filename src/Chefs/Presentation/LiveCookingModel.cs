using Chefs.Business;
using System.Collections.Immutable;
namespace Chefs.Presentation;

public record LiveCookingParameter(Recipe Recipe, IImmutableList<Step> Steps);

public partial class LiveCookingModel // DR_REV: Use Model suffix instead of ViewModel
{
    private readonly INavigator _navigator;

    public LiveCookingModel(LiveCookingParameter parameter, INavigator navigator)
    {
        _navigator = navigator;
        Steps = parameter.Steps;
        Recipe = parameter.Recipe;
    }

    // DR_REV: This seems to be used only to display the snapshot received in ctor. There is no need to wrap it into a ListFeed, use a simple property instead:
    //          public IEnumerable<Step> Steps { get; }
    public IImmutableList<Step> Steps { get; }

    public IFeed<Step> SelectedStep => SelectedIndex.Select(x => Steps[x]);

    public IState<int> SelectedIndex => State.Value(this, () => 0);

    public Recipe Recipe { get; }

	// DR_REV: XAML only nav
	public async ValueTask GoBack(CancellationToken ct) =>
        await _navigator.GoBack(this);
}
