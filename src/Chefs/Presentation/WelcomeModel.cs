namespace Chefs.Presentation;

public partial class WelcomeModel
{
	private readonly INavigator _navigator;

	public WelcomeModel(INavigator navigator)
	{
		_navigator = navigator;
	}

	public IState<int> NextPage => State.Value(this, () => 0);
	public IFeed<string> ButtonText => NextPage.Select(x => x >= 2 ? "Let's cook!" : "Next");

	public async ValueTask Next(int nextPage, CancellationToken ct)
	{
		if (nextPage >= 2)
		{
			await _navigator.NavigateViewModelAsync<LoginModel>(this, Qualifiers.ClearBackStack);
		}
		else
		{
			await NextPage.Set(nextPage + 1, ct);
		}
	}
}
