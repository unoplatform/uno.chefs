using Chefs.Business;
using System.Collections.Immutable;
namespace Chefs.Presentation;

public record LiveCookingParameter(Recipe Recipe, IImmutableList<Step> Steps);

public partial class LiveCookingModel
{
    public LiveCookingModel(LiveCookingParameter parameter)
    {
        Steps = parameter.Steps;
        Recipe = parameter.Recipe;
    }

    public IImmutableList<Step> Steps { get; }

    public IFeed<Step> SelectedStep => SelectedIndex.Select(x => Steps[x]);

    public IState<int> SelectedIndex => State.Value(this, () => 0);

    public Recipe Recipe { get; }
}
