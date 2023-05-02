namespace Chefs.Presentation;

public record LiveCookingParameter(Recipe Recipe, IImmutableList<Step> Steps);

public partial class LiveCookingModel
{
	private readonly INavigator _navigator;

	public LiveCookingModel(LiveCookingParameter parameter, INavigator navigator)
	{
		Steps = parameter.Steps;
		Recipe = parameter.Recipe;

		_navigator = navigator;
	}

	public IImmutableList<Step> Steps { get; }

	public IFeed<Step> SelectedStep => SelectedIndex.Select(x => Steps[x]);

	public IState<int> SelectedIndex => State.Value(this, () => 0);

	public IFeed<bool> CanFinish => SelectedIndex.Select(x => x == Steps.Count - 1);

	public Recipe Recipe { get; }

	public async ValueTask Complete(CancellationToken ct)
	{
		await _navigator.NavigateBackAsync(this, cancellation: ct);

		await _navigator.NavigateRouteAsync(this, "Completed", Qualifiers.Dialog, cancellation: ct);
	}
}
