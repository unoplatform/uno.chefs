namespace Chefs.Presentation;

public partial class WelcomeModel
{
	private readonly INavigator _navigator;

	private const int PageCount = 3;

	public WelcomeModel(INavigator navigator)
	{
		_navigator = navigator;
	}

	public IState<int> NextPage => State.Value(this, () => 0);

	public IFeed<bool> HasNext => NextPage.Select(x => x < PageCount - 1);

	public async ValueTask Next(int nextPage, CancellationToken ct) 
	{
		if (nextPage >= 2)
		{ 			
			await _navigator.NavigateViewModelAsync<LoginModel>(this, Qualifiers.ClearBackStack);
		}
		else
		{
			await NextPage.Set(nextPage += 1, ct);
		}
	}
}
