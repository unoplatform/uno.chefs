namespace Chefs.Presentation;

public partial record WelcomeModel(INavigator navigator)
{
	public IState<Iterator<int>> Pages => State<Iterator<int>>.Value(this, () => new Iterator<int>(Enumerable.Range(0, 3).ToImmutableList()));
	
	public async ValueTask NextPage(Iterator<int> pages)
	{
		if (!pages.CanMoveNext)
		{
			await navigator.NavigateViewModelAsync<LoginModel>(this, Qualifiers.ClearBackStack);
		}
	}
}
