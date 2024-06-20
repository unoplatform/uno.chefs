namespace Chefs.Presentation;

public partial record WelcomeModel(INavigator navigator)
{
	public IState<IntIterator> Pages => State<IntIterator>.Value(this, () => new IntIterator(Enumerable.Range(0, 3).ToImmutableList()));
	
	public async ValueTask NextPage(IntIterator pages)
	{
		if (!pages.CanMoveNext)
		{
			await navigator.NavigateViewModelAsync<LoginModel>(this, Qualifiers.ClearBackStack);
		}
	}
}
