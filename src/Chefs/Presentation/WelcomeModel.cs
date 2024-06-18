namespace Chefs.Presentation;

public partial class WelcomeModel(INavigator navigator)
{
	public IState<Iterator<int>> Pages => State.Value(this, () => new Iterator<int>(Enumerable.Range(0, 3).ToImmutableList()));
	
	public async ValueTask NextPage()
	{
		var pages = await Pages;
		if (pages?.CanMoveNext ?? false)
		{
			await Pages.UpdateAsync(p => p?.MoveNext());
		}
		else
		{
			await navigator.NavigateViewModelAsync<LoginModel>(this, Qualifiers.ClearBackStack);
		}
	}
}