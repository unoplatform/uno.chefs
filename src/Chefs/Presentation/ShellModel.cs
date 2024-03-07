namespace Chefs.Presentation;

public class ShellModel
{
	private readonly INavigator _navigator;
	private readonly IWritableOptions<Credentials> _credentialsSettings;

	public ShellModel(
		INavigator navigator,
		IWritableOptions<Credentials> credentials)
	{
		_navigator = navigator;
		_credentialsSettings = credentials;

		_ = Start();
	}

	public async Task Start() => await _navigator.NavigateViewModelAsync<SearchModel>(this);
}
