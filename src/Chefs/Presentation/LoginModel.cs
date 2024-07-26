namespace Chefs.Presentation;

public partial record LoginModel(IDispatcher Dispatcher, INavigator Navigator, IAuthenticationService Authentication)
{
	public string Title { get; } = "Login";
	
	public IState<Credentials> UserCredentials => State<Credentials>.Value(this, () => new Credentials());

    public ICommand Login => Command.Create(b => b.Given(UserCredentials).When(CanLogin).Then(DoLogin));

	private bool CanLogin(Credentials userCredentials)
	{
		return userCredentials is not null &&
			   !string.IsNullOrWhiteSpace(userCredentials.Username) &&
			   !string.IsNullOrWhiteSpace(userCredentials.Password);
	}

	private async ValueTask DoLogin(Credentials userCredentials, CancellationToken ct)
	{
		await Authentication.LoginAsync(Dispatcher, new Dictionary<string, string> { { "Username", userCredentials.Username! }, { "Password", userCredentials.Password! } });
		await Navigator.NavigateViewModelAsync<MainModel>(this, qualifier: Qualifiers.ClearBackStack);
	}
}
