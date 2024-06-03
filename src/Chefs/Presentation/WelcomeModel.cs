namespace Chefs.Presentation;

public partial class WelcomeModel
{
	private readonly Iterable<int> _pages = new(Enumerable.Range(0, 3).ToImmutableList());
	private readonly IState<Iterable<int>> Pages;
	private readonly INavigator _navigator;

	public WelcomeModel(INavigator navigator)
	{
		_navigator = navigator;
		Pages = State<Iterable<int>>.Value(this, () => _pages);
	}
	
	public bool HasNext => _pages.CanMoveNext;

	public async ValueTask NextPage()
	{
		if (HasNext)
		{
			await Pages.UpdateAsync(p => p.MoveNext());
		}
		else
		{
			await _navigator.NavigateViewModelAsync<LoginModel>(this, Qualifiers.ClearBackStack);
		}
	}
	
}
