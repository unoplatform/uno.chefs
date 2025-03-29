namespace Chefs.Presentation;

public partial record RegistrationModel(
	IDispatcher Dispatcher,
	INavigator Navigator,
	IAuthenticationService Authentication)
{
	public string Title { get; } = "Register";
	
	public IState<Credentials> UserCredentials => State<Credentials>.Value(this, () => new Credentials());
	
	public async ValueTask Register(Credentials userCredentials)
	{
		var username = userCredentials?.Username ?? string.Empty;
		var password = userCredentials?.Password ?? string.Empty;
		
		var success = await Authentication.LoginAsync(Dispatcher, new Dictionary<string, string>
		{
			{ "Username", username },
			{ "Password", password }
		});
		
		if (success)
		{
			await Navigator.NavigateViewModelAsync<MainModel>(this);
		}
	}
}
