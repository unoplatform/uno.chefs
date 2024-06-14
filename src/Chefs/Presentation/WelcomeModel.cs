namespace Chefs.Presentation;

public partial class WelcomeModel
{
	private readonly INavigator _navigator;
	
	public IState<IntIterable> Pages => State.Value(this, () => new IntIterable(Enumerable.Range(0, 3).ToImmutableList()));

	public WelcomeModel(INavigator navigator)
	{
		_navigator = navigator;
	}
	
	public async ValueTask NextPage()
	{
		var pages = await Pages;
		if (pages?.CanMoveNext ?? false)
		{
			await Pages.UpdateAsync(p => p?.MoveNext() as IntIterable);
		}
		else
		{
			await _navigator.NavigateViewModelAsync<LoginModel>(this, Qualifiers.ClearBackStack);
		}
	}
}
public record IntIterable(IImmutableList<int> Items) : Iterable<int>(Items);
